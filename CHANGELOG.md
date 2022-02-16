# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]
### Added

### Changed

### Fixed

## [1.4.0] - 2022-02-16
### Added
- Added support for Voice Link engagements
- Added `TextVoice` and `EmailVoice` kinds to `Activity` resource
- Added `CallBackPhoneNumber`, `EmailMessage`, and `VoiceTemplate` to `Activity` resource
- Added test coverage
- Updated GitHub workflow

## [1.3.0] - 2021-12-29
### Added
- Add support to `OmnigageClient.Init` for JWT tokens
- Refactored `AuthContext` to support both Basic and JWT authentication
- Added unit test coverage for JWT tokens
- Updated NuGet packages

## [1.2.0] - 2020-07-14
### Added
- Added `ValidationException` when API returns a `400` and unit test coverage
- Added `AuthException` when API returns a `401` and unit test coverage
- Refactored `Client.SendClientRequest` to use `RequestHandler` and `ResponseHandler`
- Added support for targeting .NET Framework 4.6.1

## [1.1.1] - 2020-07-10
### Fixed
- Corrected issue with `trigger` serializing default values impacting validation

### Changed
- Updated VCR cassette for engagement voice integration test

## [1.1.0] - 2020-07-10
### Added
- Added support for the following resources: `calls`, `conferences`, `emails`, `email-messages`, `email-templates`, `phone-numbers`, `texts`, `text-messages`, `text-templates`
- Expanded engagements to support email and text blasts
- Simplified client configuration
- Added integration tests
- Expanded unit tests

## [1.0.0] - 2020-06-24
### Added
- Authentication class
- Resource classes including voice template, engagement, activity, trigger, envelope, and upload
- Utilities including adapter abstract for serialization and request handling
- Initial scope focused around engagement creation for voice activities

[Unreleased]: https://github.com/omnigage/omnigage-sdk-dotnet/compare/1.2.0...HEAD
[1.2.0]: https://github.com/omnigage/omnigage-sdk-dotnet/compare/1.1.1...1.2.0
[1.1.1]: https://github.com/omnigage/omnigage-sdk-dotnet/compare/1.1.0...1.1.1
[1.1.0]: https://github.com/omnigage/omnigage-sdk-dotnet/compare/1.0.0...1.1.0
[1.0.0]: https://github.com/omnigage/omnigage-sdk-dotnet/releases/tag/1.0.0
