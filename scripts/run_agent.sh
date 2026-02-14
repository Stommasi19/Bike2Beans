#!/usr/bin/env bash
set -euo pipefail

if [[ $# -ne 1 ]]; then
  echo "Usage: $0 <backend|frontend|ui-design|product>" >&2
  exit 1
fi

agent="$1"
root_dir="$(git rev-parse --show-toplevel)"
today_utc="$(date -u +%F)"

if [[ -z "${OPENAI_API_KEY:-}" ]]; then
  echo "OPENAI_API_KEY is required." >&2
  exit 1
fi

if ! command -v jq >/dev/null 2>&1; then
  echo "jq is required." >&2
  exit 1
fi

if ! command -v curl >/dev/null 2>&1; then
  echo "curl is required." >&2
  exit 1
fi

case "$agent" in
  backend)
    spec_rel="agents/Bike2Beans_Backend_Advisor.md"
    out_rel="agents/deliverables/backend/${today_utc}_post-merge-review.md"
    ;;
  frontend)
    spec_rel="agents/Bike2Beans_Frontend_Advisor.md"
    out_rel="agents/deliverables/frontend/${today_utc}_post-merge-review.md"
    ;;
  ui-design)
    spec_rel="agents/Bike2Beans_UI_Design_Advisor.md"
    out_rel="agents/deliverables/ui-design/${today_utc}_post-merge-review.md"
    ;;
  product)
    spec_rel="agents/Bike2Beans_Product_Manager.md"
    out_rel="agents/deliverables/product/${today_utc}_post-merge-review.md"
    ;;
  *)
    echo "Unsupported agent: $agent" >&2
    exit 1
    ;;
esac

spec_abs="${root_dir}/${spec_rel}"
out_abs="${root_dir}/${out_rel}"
mkdir -p "$(dirname "$out_abs")"

event_path="${GITHUB_EVENT_PATH:-}"
pr_number=""
pr_title=""
pr_body=""
base_sha=""
merge_sha=""

if [[ -n "$event_path" && -f "$event_path" ]]; then
  pr_number="$(jq -r '.pull_request.number // ""' "$event_path")"
  pr_title="$(jq -r '.pull_request.title // ""' "$event_path")"
  pr_body="$(jq -r '.pull_request.body // ""' "$event_path")"
  base_sha="$(jq -r '.pull_request.base.sha // ""' "$event_path")"
  merge_sha="$(jq -r '.pull_request.merge_commit_sha // ""' "$event_path")"
fi

diff_range=""
use_show_mode="false"
if [[ -n "$base_sha" && -n "$merge_sha" ]] && git cat-file -e "$base_sha^{commit}" 2>/dev/null && git cat-file -e "$merge_sha^{commit}" 2>/dev/null; then
  diff_range="${base_sha}..${merge_sha}"
elif git rev-parse --verify HEAD~1 >/dev/null 2>&1; then
  diff_range="HEAD~1..HEAD"
else
  diff_range="HEAD"
  use_show_mode="true"
fi

if [[ "$use_show_mode" == "true" ]]; then
  changed_files="$(git show --name-only --pretty="" HEAD | head -n 200)"
  diff_stat="$(git show --stat --pretty="" HEAD | head -n 200)"
  patch_excerpt="$(git show --unified=1 --pretty="" HEAD | head -c 60000)"
else
  changed_files="$(git diff --name-only "$diff_range" | head -n 200)"
  diff_stat="$(git diff --stat "$diff_range" | head -n 200)"
  patch_excerpt="$(git diff --unified=1 "$diff_range" | head -c 60000)"
fi

prompt_file="$(mktemp)"
response_file="$(mktemp)"

cat >"$prompt_file" <<EOF
You are running the Bike2Beans "${agent}" advisor.
Follow this advisor spec as hard constraints:

<advisor_spec>
$(cat "$spec_abs")
</advisor_spec>

Task:
Generate one markdown report for a PR that was merged into main.
Write concrete, project-specific analysis and recommendations based on the merged changes.
Do not include code blocks unless necessary.
Primary objective: maximize the user's learning from this merged PR.

Output requirements:
- Return markdown only.
- Start with a top-level heading that includes the agent name and date.
- Keep sections aligned with the advisor spec's required structure.
- Include file path references when relevant.
- Explain key reasoning in plain language and avoid unexplained jargon.
- End with a `Learning Sprint` section containing:
  1) top 3 concepts to learn next,
  2) one 30-minute practice task,
  3) one self-check question with expected answer outline.

Repository context:
- Repo: Bike2Beans
- PR Number: ${pr_number}
- PR Title: ${pr_title}
- PR Body:
${pr_body}

Changed files:
${changed_files}

Diff stat:
${diff_stat}

Diff excerpt:
${patch_excerpt}
EOF

model="${OPENAI_MODEL:-gpt-5-mini}"
payload="$(jq -n \
  --arg model "$model" \
  --arg input "$(cat "$prompt_file")" \
  '{
      model: $model,
      input: [
        {
          role: "user",
          content: [
            { type: "input_text", text: $input }
          ]
        }
      ]
    }')"

curl -sS https://api.openai.com/v1/responses \
  -H "Authorization: Bearer ${OPENAI_API_KEY}" \
  -H "Content-Type: application/json" \
  -d "$payload" >"$response_file"

report_text="$(jq -r '
  ([
    .output[]?.content[]?
    | select(.type=="output_text")
    | .text
  ] | join("\n"))
  // .output_text
  // empty
' "$response_file")"

if [[ -z "$report_text" ]]; then
  echo "No report text returned from OpenAI API." >&2
  jq -C . "$response_file" >&2 || true
  exit 1
fi

printf '%s\n' "$report_text" >"$out_abs"
echo "Wrote report: $out_rel"

if [[ -n "${GITHUB_STEP_SUMMARY:-}" ]]; then
  {
    echo "## ${agent} advisor report"
    echo
    echo "- Output: \`${out_rel}\`"
    echo "- Diff range: \`${diff_range}\`"
  } >>"${GITHUB_STEP_SUMMARY}"
fi

rm -f "$prompt_file" "$response_file"
