
## [v1.1.0] - 2026-03-24
### :sparkles: New Features
- [`ee42481`](https://github.com/cmpnnt/streamdeck-toolkit/commit/ee42481076d2469f9e50f74f22db5fa7a5548b7c) - **manifest**: implement automatic manifest.json generation from source *(commit by [@cmpnnt](https://github.com/cmpnnt))*

### :wrench: Chores
- [`722be73`](https://github.com/cmpnnt/streamdeck-toolkit/commit/722be733d010bea2bcc3ce17b48d883962cd7ec9) - **docs**: remove old documentation and CI configuration files *(commit by [@cmpnnt](https://github.com/cmpnnt))*


## [v1.0.3] - 2026-03-23
### :recycle: Refactors
- [`6e6fdab`](https://github.com/cmpnnt/streamdeck-toolkit/commit/6e6fdabb77ebcbde8f20435d90b5f718aaa5fc3d) - reorganize Cmpnnt.StreamDeckToolkit directory structure *(commit by [@cmpnnt](https://github.com/cmpnnt))*


## [v1.0.2] - 2026-03-23
### :recycle: Refactors
- [`9036e93`](https://github.com/cmpnnt/streamdeck-toolkit/commit/9036e9345cb05b84bf185f1356134ca3c9c30a4e) - **build**: move namespaces and upgrade framework/language versions *(commit by [@cmpnnt](https://github.com/cmpnnt))*

### :wrench: Chores
- [`96c2a1c`](https://github.com/cmpnnt/streamdeck-toolkit/commit/96c2a1c8fa06e6364dbb967fd1fa5432f4b4c9ab) - **warnings**: address compiler warnings *(commit by [@cmpnnt](https://github.com/cmpnnt))*


## [v1.0.1] - 2026-03-23
### :bug: Bug Fixes
- [`5759799`](https://github.com/cmpnnt/streamdeck-tools/commit/5759799d70a5a4caee0ae6e7bc3466a2b5671da4) - **build**: replace Environment.OSVersion with RuntimeInformation for macOS compatibility *(commit by [@cmpnnt](https://github.com/cmpnnt))*


## [v1.0.0] - 2026-03-23
### :boom: BREAKING CHANGES
- due to [`995af06`](https://github.com/cmpnnt/streamdeck-tools/commit/995af06e73e211fd6e8d2b07d9175bf74b4d4657) - simplify event routing and fix AOT-safe BaseEvent deserialization *(commit by [@cmpnnt](https://github.com/cmpnnt))*:

  plugin actions must implement new interface methods instead of subscribing to SdConnection events


### :sparkles: New Features
- [`291baa2`](https://github.com/cmpnnt/streamdeck-tools/commit/291baa27506dd973b56436c3f1a350592617c7ad) - switch from attributes to inheritance based plugin auto registration *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`aa1df24`](https://github.com/cmpnnt/streamdeck-tools/commit/aa1df24c2c24bdcc922e0c29aa45ce670e20c3c8) - add new devices *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`5746069`](https://github.com/cmpnnt/streamdeck-tools/commit/5746069d892a32d20a6e2eba3ca95b48951c5174) - manifest auto generation *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`9443939`](https://github.com/cmpnnt/streamdeck-tools/commit/94439393039f4673597de915f47f64cc13c9511c) - removed PluginActionIdAttribute *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`efc6b47`](https://github.com/cmpnnt/streamdeck-tools/commit/efc6b47776ad37cf9329a9d0d34d9b83c6bf3543) - switch to system.text.json *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`d29f263`](https://github.com/cmpnnt/streamdeck-tools/commit/d29f2630dc9a204f89109f0a72efa5d3c21a5f8d) - implement source generation for property inspector *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`2cb9dcb`](https://github.com/cmpnnt/streamdeck-tools/commit/2cb9dcb93399dce55422dc46cfdeae0265a2b4f6) - implement source-generated AOT-safe settings population *(commit by [@cmpnnt](https://github.com/cmpnnt))*

### :bug: Bug Fixes
- [`4f18c1c`](https://github.com/cmpnnt/streamdeck-tools/commit/4f18c1cd7c524a01b7ecea122f0ad808c3b9b1da) - case sensitivity causing actions not to load *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`c1efd12`](https://github.com/cmpnnt/streamdeck-tools/commit/c1efd125019b50f40da5278f7f0b7c091c134a54) - open/close stream deck software on build *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`eea8cf0`](https://github.com/cmpnnt/streamdeck-tools/commit/eea8cf09b8d891b7c803443ff45d9a5d7296b643) - implement proper disposal pattern for StreamDeckConnection *(commit by [@cmpnnt](https://github.com/cmpnnt))*

### :recycle: Refactors
- [`74d73cc`](https://github.com/cmpnnt/streamdeck-tools/commit/74d73cc56eaec587487f1940366e4c4a6f1c447c) - additional error logging *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`4fb2f0c`](https://github.com/cmpnnt/streamdeck-tools/commit/4fb2f0c508e274696108b5c9a10f0e98b249a26f) - adjust project names and namespaces *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`995af06`](https://github.com/cmpnnt/streamdeck-tools/commit/995af06e73e211fd6e8d2b07d9175bf74b4d4657) - simplify event routing and fix AOT-safe BaseEvent deserialization *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`c98ef1c`](https://github.com/cmpnnt/streamdeck-tools/commit/c98ef1cb26e110e429c1c4e2508c58f400b8a473) - **PluginContainer**:  *(commit by [@cmpnnt](https://github.com/cmpnnt))*

### :wrench: Chores
- [`88a4e6b`](https://github.com/cmpnnt/streamdeck-tools/commit/88a4e6b52be86806349a530c50272aabfcad070a) - remove FUNDING.yml *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`024c6a8`](https://github.com/cmpnnt/streamdeck-tools/commit/024c6a81a1fd2ca0884490eeb1fc94bcb84bed81) - update LICENSE *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`81cb754`](https://github.com/cmpnnt/streamdeck-tools/commit/81cb7548cbb70e27954f77be6e9cfb857ccb9b7d) - remove visual studio template *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`5fd9314`](https://github.com/cmpnnt/streamdeck-tools/commit/5fd9314361cdef39b32cff7e0c5c43395a8a2d2b) - comment formatting *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`1436dc1`](https://github.com/cmpnnt/streamdeck-tools/commit/1436dc1db86ff90f69aec161cb0b24e4c17b30a4) - add configuration and platforms *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`a16c178`](https://github.com/cmpnnt/streamdeck-tools/commit/a16c1787850af3fd4c640eb0f9166ae8dbe7ac40) - add doc comments *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`693f994`](https://github.com/cmpnnt/streamdeck-tools/commit/693f994b74c5cd9560801cecd340e2cf8cd8f289) - add .streamDeckPlugin files to .gitignore *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`8931d0e`](https://github.com/cmpnnt/streamdeck-tools/commit/8931d0e19171c4f450c5ae586a02eef1bd764b95) - remove sample manifest copy directive *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`82b3c2d`](https://github.com/cmpnnt/streamdeck-tools/commit/82b3c2d1dcd02889361ca2946f45a5ea0e9c9ed8) - formatting *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`0a07542`](https://github.com/cmpnnt/streamdeck-tools/commit/0a0754283f66ba1281788c2b6cd18a4ec5990a78) - add comments to sample csproj *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`82d9075`](https://github.com/cmpnnt/streamdeck-tools/commit/82d9075ea361dd4526cf27bc69348eef9bff57b7) - move CloseStreamDeck action to separate target *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`8054a9f`](https://github.com/cmpnnt/streamdeck-tools/commit/8054a9f2ee190af25c341570a5b5ee32817b7d08) - add kill-msbuild.ps1 for Rider's nonsense *(commit by [@cmpnnt](https://github.com/cmpnnt))*

[v1.0.0]: https://github.com/cmpnnt/streamdeck-tools/compare/v0.1.0...v1.0.0
[v1.0.1]: https://github.com/cmpnnt/streamdeck-tools/compare/v1.0.0...v1.0.1
[v1.0.2]: https://github.com/cmpnnt/streamdeck-toolkit/compare/v1.0.1...v1.0.2
[v1.0.3]: https://github.com/cmpnnt/streamdeck-toolkit/compare/v1.0.2...v1.0.3
[v1.1.0]: https://github.com/cmpnnt/streamdeck-toolkit/compare/v1.0.3...v1.1.0
