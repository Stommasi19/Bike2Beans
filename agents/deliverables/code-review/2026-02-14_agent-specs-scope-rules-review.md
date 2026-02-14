# Code Review Report: agent-specs-scope-rules

## 1. What the Change Does

Adds a new `agents/` workspace with five role-specific advisor specs, a shared `agents/README.md` operating guide, and placeholder deliverable directories under `agents/deliverables/*`.

## 2. Findings by Severity (Critical, High, Medium, Low)

### Critical

- None.

### High

- None.

### Medium

1. **Ambiguous `<agent-name>` placeholder can cause branch/path drift across agents.**
   - Evidence: `/Users/sebastiantommasi/.codex/worktrees/4641/Bikes2Beans/agents/README.md:16` and `/Users/sebastiantommasi/.codex/worktrees/4641/Bikes2Beans/agents/README.md:18` use generic `<agent-name>`, while actual agent identifiers vary by file naming style (e.g., `Bike2Beans_Product_Manager.md`, `Bike2Beans_UI_Design_Advisor.md`).
   - Risk: automation or human execution may choose inconsistent slugs (`product-manager` vs `product`, `ui-design` vs `ui_design`), causing misrouted deliverables and branch policy violations.

2. **Product advisor scope allows top-level `README.md` edits from a non-engineering lane.**
   - Evidence: `/Users/sebastiantommasi/.codex/worktrees/4641/Bikes2Beans/agents/Bike2Beans_Product_Manager.md:24`.
   - Risk: cross-domain documentation churn and mixed-objective PRs, conflicting with focused-scope rule in `/Users/sebastiantommasi/.codex/worktrees/4641/Bikes2Beans/agents/README.md:19`.

### Low

1. **Design-agent implementation guardrails are policy-only (not enforceable by path).**
   - Evidence: `/Users/sebastiantommasi/.codex/worktrees/4641/Bikes2Beans/agents/Bike2Beans_UI_Design_Advisor.md:24` allows `Bike2BeansUI/**` with a prose-only restriction.
   - Risk: accidental production code edits can pass as in-scope without automated checks.

## 3. Architecture Concerns

- Governance is split between one shared policy file and five role specs, but key identifiers (agent slug naming) are not normalized in one canonical mapping.
- This creates avoidable coordination failures when these rules are consumed by agents, scripts, or CI policy checks.

## 4. Performance and Cost Concerns

- No direct runtime performance impact from this change.
- Indirect cost risk exists if ambiguous scopes trigger rework cycles (reruns, re-reviews, PR churn).

## 5. Security Concerns

- No direct credential/data-exposure issue introduced by these files.
- Broad documentation edit permissions (product spec) increase process risk, not direct security risk.

## 6. Maintainability Concerns

- Repeated policy text across multiple spec files can drift over time without a single source of truth.
- Deliverable/branch naming conventions rely on interpretation instead of a strict, shared slug table.

## 7. Recommended Next Actions

1. Add a canonical slug matrix in `agents/README.md` (e.g., `backend`, `frontend`, `ui-design`, `product`, `code-review`) and require its use for branch + deliverable paths.
2. Narrow product scope from `README.md` to an explicit path set (for example `agents/deliverables/product/**` and `docs/product/**`) unless broader edits are explicitly requested.
3. Add lightweight CI checks that validate branch prefix and deliverable path against the agent slug to prevent policy drift.
