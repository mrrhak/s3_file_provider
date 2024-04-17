# Contributing to icons_launcher

Thank you for your interest in contributing!

Contributors not only contribute code but also help by submitting issues, contributing to issue discussions and helping to answer questions on StackOverflow.

This document outlines the rules and procedures when contributing.

## How we use GitHub

We use GitHub as a place and tool for contributors to work together on project development. That is, we use it to report bugs, suggest improvements, and contribute code and documentation to address those bugs or suggestions.

## Reporting a bug

1. **Is it a bug?** Check first that you're using the API correctly according to the documentation (README and API documentation). If the API crashes when used incorrectly, you should not report a bug.
2. **Is it a new bug?** Search the GitHub issues page for bugs. If this bug has already been reported, you should not report a new bug.
3. **How can we reproduce it?** Fork this repository and make the "minimal" changes necessary to the example to reproduce the bug. This step is unnecessary if you choose to fix the bug yourself, or if the example already exhibits the bug without modification.
4. **Submit a report!** With all of the information you have collected you can submit a bug report via the "New issue" page on GitHub. It is necessary to fill in all required information provided by the template in the same format as the template to avoid automatic closure of the issue.

Things to AVOID:

* Do not share your whole app as the minimal reproduction project. This is not "minimal" and it makes it difficult to understand what's happening.
* Do not use a bug report to ask a question. Use StackOverflow instead.
* Do not submit a bug report if you are not using the APIs correctly according to the documentation.
* Try to avoid posting a duplicate bug.
* Do not ignore the formatting requirements and instructions within the issue template.

## Suggesting an improvement

The GitHub "New issue" page provides 2 templates for suggesting improvements: Feature request and Bug report. In both cases, make sure you search existing issues to prevent yourself from posting a duplicate, and ensure that you fill in all required sections in the issue template keeping the original formatting to avoid automatic closure.

## Making a pull request

Pull requests are used to contribute bug fixes, new features or documentation improvements. Before working on a pull request, an issue should exist that describes the feature or bug.

To create a pull request:

1. Fork this repository
2. Create a branch for your changes. Branch off the `master` branch if introducing a breaking change that is not backwards compatible if making a non-breaking change or bug fix.
3. Make your changes, updating the documentation if you have changed any API's behavior.
4. If you are the first to contribute to the next version, increment the version number in `pubspec.yaml` according to the [pub versioning philosophy](https://dart.dev/tools/pub/versioning).
5. Add a description of your change to `CHANGELOG.md` (format: `* DESCRIPTION OF YOUR CHANGE (@your-git-username)`).
6. Run `flutter analyze` to ensure your code meets the static analysis requirements.
7. Create the pull request via [GitHub's instructions](https://docs.github.com/en/github/collaborating-with-issues-and-pull-requests/creating-a-pull-request-from-a-fork).
8. [link](https://docs.github.com/en/github/managing-your-work-on-github/linking-a-pull-request-to-an-issue) that pull request with the original issue.

Best practices for a pull request:

* Follow the style of existing code, paying attention to whitespace and formatting. (In Dart, we use `dart format` which may already be integrated into your IDE.)
* Check your diffs prior to submission. Try to make your pull request diff reflect only the lines of code that needed to be changed to address the issue at hand. If your diff also changes other things, such as by making superficial changes to code formatting and layout, or checking in superfluous files such as your IDE or other config files, please remove them so that your diff focus on the essential. That helps keep the commit history clean and also makes your pull request easier to review.
* Try not to introduce multiple unrelated changes in a single pull request. Create individual pull requests so that they can be evaluated and accepted independently.
* Use meaningful commit messages so that your changes can be more easily browsed and referenced.