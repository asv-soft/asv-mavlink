# 🖥 For Developers

## For developers

## Contributing

We would love for you to contribute to our projects and help make it even better than it is today! As a contributor, here are the guidelines we would like you to follow:

* [Submission Guidelines](for-developers.md#submit)
* [Missing a feature?](for-developers.md#feature)
* [Coding rules](for-developers.md#rules)
* [Commit Message Guidelines](for-developers.md#commit)
* [Branches naming](for-developers.md#branches)
* [Semantic versioning](for-developers.md#semver)

### Found a Bug? <a href="#issue" id="issue"></a>

If you find a bug in the source code, you can help us by [submitting an issue](for-developers.md#submit-issue) to our [GitHub Repository](https://github.com/asv-soft). Even better, you can submit a Pull Request with a fix.

### Missing a Feature? <a href="#feature" id="feature"></a>

You can _request_ a new feature by [submitting an issue](for-developers.md#submit-issue) to our GitHub Repository. If you would like to _implement_ a new feature, please consider the size of the change in order to determine the right steps to proceed:

*   For a **Major Feature**, first open an issue and outline your proposal so that it can be discussed. This process allows us to better coordinate our efforts, prevent duplication of work, and help you to craft the change so that it is successfully accepted into the project.

    **Note**: Adding a new topic to the documentation, or significantly re-writing a topic, counts as a major feature.
* **Small Features** can be crafted and directly submitted as a Pull Request.

### Submission Guidelines <a href="#submit" id="submit"></a>

#### Submitting an Issue <a href="#submit-issue" id="submit-issue"></a>

Before you submit an issue, please search the issue tracker. An issue for your problem might already exist and the discussion might inform you of workarounds readily available.

We want to fix all the issues as soon as possible, but before fixing a bug, we need to reproduce and confirm it. In order to reproduce bugs, we require that you provide a minimal reproduction. Having a minimal reproducible scenario gives us a wealth of important information without going back and forth to you with additional questions.

A minimal reproduction allows us to quickly confirm a bug (or point out a coding problem) as well as confirm that we are fixing the right problem.

We require a minimal reproduction to save maintainers' time and ultimately be able to fix more bugs. Often, developers find coding problems themselves while preparing a minimal reproduction. We understand that sometimes it might be hard to extract essential bits of code from a larger codebase, but we really need to isolate the problem before we can fix it.

Unfortunately, we are not able to investigate / fix bugs without a minimal reproduction, so if we don't hear back from you, we are going to close an issue that doesn't have enough info to be reproduced.

### Coding Rules <a href="#rules" id="rules"></a>

To ensure consistency throughout the source code, keep these rules in mind as you are working:

* All features or bug fixes **must be tested** by one or more specs (unit-tests).
* All public API methods **must be documented**.

### Commit Message Format <a href="#commit" id="commit"></a>

_This specification is inspired by and supersedes the_ [_AngularJS commit message format_](https://docs.google.com/document/d/1QrDFcIiPjSLDn3EL15IJygNPiHORgU1\_OOAqWjiDU5Y/edit)_._

We have very precise rules over how our Git commit messages must be formatted. This format leads to **easier to read commit history**.

Each commit message consists of a **header**, a **body**, and a **footer**.

```
<header>
<BLANK LINE>
<body>
<BLANK LINE>
<footer>
```

The `header` is mandatory and must conform to the [Commit Message Header](for-developers.md#commit-header) format.

The `body` is mandatory for all commits except for those of type "docs". When the body is present it must be at least 20 characters long and must conform to the [Commit Message Body](for-developers.md#commit-body) format.

The `footer` is mandatory for all commits. The [Commit Message Footer](for-developers.md#commit-footer) format describes what the footer is used for and the structure it must have.

**Commit Message Header**

```
<type>(<scope>): <short summary>
  │       │             │
  │       │             └─⫸ Summary in present tense. Not capitalized. No period at the end.
  │       │
  │       └─⫸ Commit Scope: asv-common|asv-common-test|asv-cfg|asv-cfg-test|asv-drones|asv-drones-docs|asv-drones-gui|asv-drones-gbs| ... etc
  │
  └─⫸ Commit Type: build|ci|docs|feat|fix|perf|refactor|test
```

The `<type>` and `<summary>` fields are mandatory, the `(<scope>)` field is optional.

**Type**

Must be one of the following:

* **build**: Changes that affect the build system or external dependencies
* **ci**: Changes to our CI configuration files and scripts
* **docs**: Documentation only changes
* **feat**: A new feature
* **fix**: A bug fix
* **perf**: A code change that improves performance
* **refactor**: A code change that neither fixes a bug nor adds a feature
* **test**: Adding missing tests or correcting existing tests

**Scope**

The scope should be the name of the project affected (as perceived by the person reading the changelog generated from commit messages). If the scope of changes is project scope you can skip writing this. For all other scopes - you can skip main project scope e.g: asv-drones-gui-core -> core.

The following is the list of supported scopes:

* `asv-common`
* `asv-common-test`
* `asv-cfg`
* `asv-cfg-test`
* `asv-drones`
* `asv-drones-docs`
* `asv-drones-gui`
* `asv-drones-gbs`
* `asv-drones-gui-core`
* `asv-drones-gui-gbs`
* `asv-drones-gui-map`
* `asv-drones-gui-sdr`
* `asv-drones-gui-uav`
* `asv-gnss`
* `asv-gnss-prometheus`
* `asv-gnss-shell`
* `asv-gnss-test`
* `asv-io`
* `asv-io-test`
* `asv-mavlink`
* `asv-mavlink-shell`
* `asv-mavlink-test`
* `asv-store`
* `asv-store-test`

**Summary**

Use the summary field to provide a succinct description of the change:

* use the imperative, present tense: "change" not "changed" nor "changes"
* don't capitalize the first letter
* no dot (.) at the end

**Commit Message Body**

Just as in the summary, use the imperative, present tense: "fix" not "fixed" nor "fixes".

Explain the motivation for the change in the commit message body. This commit message should explain _why_ you are making the change. You can include a comparison of the previous behavior with the new behavior in order to illustrate the impact of the change.

**Commit Message Footer**

The footer can contain information about breaking changes and deprecations and is also the place to reference GitHub issues, Asana tickets, and other PRs that this commit closes or is related to. For example:

```
BREAKING CHANGE: <breaking change summary>
<BLANK LINE>
<breaking change description + migration instructions>
<BLANK LINE>
<BLANK LINE>
Fixes #<issue number>
```

or

```
DEPRECATED: <what is deprecated>
<BLANK LINE>
<deprecation description + recommended update path>
<BLANK LINE>
<BLANK LINE>
Closes #<pr number>
```

Breaking Change section should start with the phrase "BREAKING CHANGE: " followed by a summary of the breaking change, a blank line, and a detailed description of the breaking change that also includes migration instructions.

Similarly, a Deprecation section should start with "DEPRECATED: " followed by a short description of what is deprecated, a blank line, and a detailed description of the deprecation that also mentions the recommended update path.

Every commit must contain footer, for our team developers footer must contain reference of a task in Asana, for common contributors - reference of an issue on GitHub.

For example:

Our team commit:

```
fix(asv-drones-gui-uav): change drones goto functionality

Change scope of some variables of an anchor to public

Asana: https://app.asana.com/0/12345678901234/1234567890123456/f 
```

Common contributor commit:

```
fix(asv-drones-gui-uav): change drones goto functionality

Change scope of some variables of an anchor to public

Issue: https://github.com/asv-soft/asv-drones/issues/1234   
```

#### Revert commits

If the commit reverts a previous commit, it should begin with `revert:` , followed by the header of the reverted commit.

The content of the commit message body should contain:

* information about the SHA of the commit being reverted in the following format: `This reverts commit <SHA>`,
* a clear description of the reason for reverting the commit message.

### Branches naming <a href="#branches" id="branches"></a>

There is a short list of branch names to create:

* (Feature) - used when adding new functionality on branch
* (Hotfix) - used when fixing existed functionality on branch

### Semantic versioning <a href="#semver" id="semver"></a>

We use semantic versioning in our projects. If you want to read more about it - try visit [this site](https://semver.org/).