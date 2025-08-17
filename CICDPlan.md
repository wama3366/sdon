# Git branching plan
Only one release is allowed to go through QA/Staging at a time. This does not address the situation where releases or 
bugs need to be interweaved on QA/Staging environments. Using containers and/or Infrastructure as code to
spin up ad-hoc QA and Staging environments maybe a way to handle that scenario.

## Definitions
### Branches
- production: contains code currently in production.
- develop: contains latest reviewed code.
- releases/v1.2.3/release: contains code frozen from develop to eventually get merged with main and released.
- releases/v1.2.3/bugfixes/bugfix-name: contains code to fix a bug in a release branch.
- features/feature-name: contains code pertaining to a specific feature. Not yet merged into develop.
- hotfixes/hotfix-name: contains code to fix a bug in production.

### Tests
- Basic tests: Unit tests. Linting. Mocked DB access. Mocked 3rd Party Service. 
- Advanced tests: Unit tests. Linting. Real DB access. 3rd Party test harnesses or testing accounts.
- All tests: Advanced tests. Performance. Security. Compliance. what else??

## Branch Lifetimes and CI/CD
### Feature lifetime
Branch off of develop
Push changes to feature branch -> runs basic tests.
Merge back into develop -> runs advanced tests.
Delete feature branch.

### Release lifetime
Branch off of develop
Deploy to QA -> runs all tests.
Deploy to Staging -> runs all tests
After all bugs are fixed:
Merge into main -> runs all tests
Deploy to Production -> runs all tests (Maybe just a subset related to deployment considerations since all tests were
already run at merge)
Delete release branch (or maintain to support multiple versions, or may just use tags on main?)

### Release bugfix lifetime
Branch off of release branch.
Merge into release branch -> runs advanced tests.
Merge into develop => runs advanced tests.
Delete bugfix branch.

### Production hotfix lifetime
Branch off main.
Deploy to QA -> runs all tests.
Deploy to Staging -> runs all tests.
Merge into main -> runs all tests.
Deploy to Production -> runs all tests (Maybe just a subset related to deployment considerations since all tests were
already run at merge)
Marge into develop -> runs advanced tests.

# Release Handling
Main release branch under /releases/v1.2.x/release-v1.2.x
Tag the top commit with release version 1.2.4
When fixing bugs in release branch, merge the fix branch in the release branch and tag the merge commit 
with version 1.2.5
Cherry pick the fix commit into develop, newer releases like /releases/v2.1.x/release-v2.1.x