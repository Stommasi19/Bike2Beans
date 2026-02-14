# Bike2Beans Agent Workspace

This folder is the shared home for all Bike2Beans agents.

## Agent Specs

- `Bike2Beans_Backend_Advisor.md`
- `Bike2Beans_Frontend_Advisor.md`
- `Bike2Beans_UI_Design_Advisor.md`
- `Bike2Beans_Product_Manager.md`
- `Bike2Beans_Code_Review_Advisor.md`

## Standard Operating Rules (All Agents)

1. Use a dedicated worktree per agent.
2. Use branch names with this format: `codex/<agent-name>/<short-topic>`.
3. Keep changes inside the agent's allowed write scope (defined in each spec).
4. Create at least one markdown deliverable in `agents/deliverables/<agent-name>/`.
5. Keep PRs focused to one objective; do not mix backend/frontend/product/design in one PR.
6. If blocked by missing context, write assumptions at the top of the deliverable and continue.

## Deliverable Naming

Use this file format for agent outputs:

`YYYY-MM-DD_<short-topic>.md`

Example:

`agents/deliverables/backend/2026-02-14_google-api-cost-controls.md`
