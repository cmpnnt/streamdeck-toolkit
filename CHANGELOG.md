

## [v1.0.0] - 2026-03-26
### :boom: BREAKING CHANGES
- due to [`e3aa29c`](https://github.com/cmpnnt/streamdeck-toolkit/commit/e3aa29c9d32ca649bffed04a1e0743a0d809dfbb) - simplify event routing and fix AOT-safe BaseEvent deserialization *(commit by [@cmpnnt](https://github.com/cmpnnt))*:

  plugin actions must implement new interface methods instead of subscribing to SdConnection events


### :sparkles: New Features
- [`291baa2`](https://github.com/cmpnnt/streamdeck-toolkit/commit/291baa27506dd973b56436c3f1a350592617c7ad) - switch from attributes to inheritance based plugin auto registration *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`aa1df24`](https://github.com/cmpnnt/streamdeck-toolkit/commit/aa1df24c2c24bdcc922e0c29aa45ce670e20c3c8) - add new devices *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`5746069`](https://github.com/cmpnnt/streamdeck-toolkit/commit/5746069d892a32d20a6e2eba3ca95b48951c5174) - manifest auto generation *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`9443939`](https://github.com/cmpnnt/streamdeck-toolkit/commit/94439393039f4673597de915f47f64cc13c9511c) - removed PluginActionIdAttribute *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`efc6b47`](https://github.com/cmpnnt/streamdeck-toolkit/commit/efc6b47776ad37cf9329a9d0d34d9b83c6bf3543) - switch to system.text.json *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`d29f263`](https://github.com/cmpnnt/streamdeck-toolkit/commit/d29f2630dc9a204f89109f0a72efa5d3c21a5f8d) - implement source generation for property inspector *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`8cd2612`](https://github.com/cmpnnt/streamdeck-toolkit/commit/8cd26125ffc5a04bf88ac581e3c056ef919964be) - implement source-generated AOT-safe settings population *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`73fe8b7`](https://github.com/cmpnnt/streamdeck-toolkit/commit/73fe8b7c4abd9d71f1202ee3eb8b9d4af0405573) - **manifest**: implement automatic manifest.json generation from source *(commit by [@cmpnnt](https://github.com/cmpnnt))*

### :bug: Bug Fixes
- [`4f18c1c`](https://github.com/cmpnnt/streamdeck-toolkit/commit/4f18c1cd7c524a01b7ecea122f0ad808c3b9b1da) - case sensitivity causing actions not to load *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`c1efd12`](https://github.com/cmpnnt/streamdeck-toolkit/commit/c1efd125019b50f40da5278f7f0b7c091c134a54) - open/close stream deck software on build *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`eea8cf0`](https://github.com/cmpnnt/streamdeck-toolkit/commit/eea8cf09b8d891b7c803443ff45d9a5d7296b643) - implement proper disposal pattern for StreamDeckConnection *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`c7584d6`](https://github.com/cmpnnt/streamdeck-toolkit/commit/c7584d6067c743c66c88e99ac00f0a9f6edc57ac) - **build**: replace Environment.OSVersion with RuntimeInformation for macOS compatibility *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`0613eb7`](https://github.com/cmpnnt/streamdeck-toolkit/commit/0613eb72f80eebde35aea3b5334dea886f24d954) - **manifest**: correctly emit codePathMac *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`11f3b1d`](https://github.com/cmpnnt/streamdeck-toolkit/commit/11f3b1dd9b8b2ac8b6615b5230befaebc0b4c127) - **AOT**: upgrade to NLog 6.x *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`ad511d4`](https://github.com/cmpnnt/streamdeck-toolkit/commit/ad511d4b01d0f0c94da69f062d6d61b0aab8b54a) - **build**: fix compiled binary name casing *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`b0576d7`](https://github.com/cmpnnt/streamdeck-toolkit/commit/b0576d7e636ea5f360696a5eb1d2a3f0195c737b) - **build**: fix publish/package plugin msbuild *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`9a9c542`](https://github.com/cmpnnt/streamdeck-toolkit/commit/9a9c542afb30df8163628ae3b38d73b233450fd1) - **AOT**: remove CommandLineParser package *(commit by [@cmpnnt](https://github.com/cmpnnt))*

### :recycle: Refactors
- [`74d73cc`](https://github.com/cmpnnt/streamdeck-toolkit/commit/74d73cc56eaec587487f1940366e4c4a6f1c447c) - additional error logging *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`4fb2f0c`](https://github.com/cmpnnt/streamdeck-toolkit/commit/4fb2f0c508e274696108b5c9a10f0e98b249a26f) - adjust project names and namespaces *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`e3aa29c`](https://github.com/cmpnnt/streamdeck-toolkit/commit/e3aa29c9d32ca649bffed04a1e0743a0d809dfbb) - simplify event routing and fix AOT-safe BaseEvent deserialization *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`de7fbb5`](https://github.com/cmpnnt/streamdeck-toolkit/commit/de7fbb5923bdf95d7ea00d4af5d1370f6a007eef) - **PluginContainer**: Extract common functionality in PluginContainer.cs for better readability *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`fbfc1c0`](https://github.com/cmpnnt/streamdeck-toolkit/commit/fbfc1c00e9317d691502967f6aa70dfdcc9e99ff) - **build**: move namespaces and upgrade framework/language versions *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`86c8723`](https://github.com/cmpnnt/streamdeck-toolkit/commit/86c87235eb21440aa304bcf6e2876a36a50155e6) - reorganize Cmpnnt.StreamDeckToolkit directory structure *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`2a323be`](https://github.com/cmpnnt/streamdeck-toolkit/commit/2a323beaf945d261ae3c02ac82afd795aa5aa684) - **naming**: rename SdWrapper to Toolkit *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`c2d5682`](https://github.com/cmpnnt/streamdeck-toolkit/commit/c2d5682ce7bdc35829ac65b9946bb11e58b4b082) - **solution**: rename slnx to match project name *(commit by [@cmpnnt](https://github.com/cmpnnt))*

### :white_check_mark: Tests
- [`cf9a452`](https://github.com/cmpnnt/streamdeck-toolkit/commit/cf9a4524f34c3d29e4e88ec8235dee15ce020197) - add event tests and move to new class *(commit by [@cmpnnt](https://github.com/cmpnnt))*

### :wrench: Chores
- [`88a4e6b`](https://github.com/cmpnnt/streamdeck-toolkit/commit/88a4e6b52be86806349a530c50272aabfcad070a) - remove FUNDING.yml *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`024c6a8`](https://github.com/cmpnnt/streamdeck-toolkit/commit/024c6a81a1fd2ca0884490eeb1fc94bcb84bed81) - update LICENSE *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`81cb754`](https://github.com/cmpnnt/streamdeck-toolkit/commit/81cb7548cbb70e27954f77be6e9cfb857ccb9b7d) - remove visual studio template *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`5fd9314`](https://github.com/cmpnnt/streamdeck-toolkit/commit/5fd9314361cdef39b32cff7e0c5c43395a8a2d2b) - comment formatting *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`1436dc1`](https://github.com/cmpnnt/streamdeck-toolkit/commit/1436dc1db86ff90f69aec161cb0b24e4c17b30a4) - add configuration and platforms *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`a16c178`](https://github.com/cmpnnt/streamdeck-toolkit/commit/a16c1787850af3fd4c640eb0f9166ae8dbe7ac40) - add doc comments *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`693f994`](https://github.com/cmpnnt/streamdeck-toolkit/commit/693f994b74c5cd9560801cecd340e2cf8cd8f289) - add .streamDeckPlugin files to .gitignore *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`8931d0e`](https://github.com/cmpnnt/streamdeck-toolkit/commit/8931d0e19171c4f450c5ae586a02eef1bd764b95) - remove sample manifest copy directive *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`82b3c2d`](https://github.com/cmpnnt/streamdeck-toolkit/commit/82b3c2d1dcd02889361ca2946f45a5ea0e9c9ed8) - formatting *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`0a07542`](https://github.com/cmpnnt/streamdeck-toolkit/commit/0a0754283f66ba1281788c2b6cd18a4ec5990a78) - add comments to sample csproj *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`82d9075`](https://github.com/cmpnnt/streamdeck-toolkit/commit/82d9075ea361dd4526cf27bc69348eef9bff57b7) - move CloseStreamDeck action to separate target *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`8054a9f`](https://github.com/cmpnnt/streamdeck-toolkit/commit/8054a9f2ee190af25c341570a5b5ee32817b7d08) - add kill-msbuild.ps1 for Rider's nonsense *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`1167f36`](https://github.com/cmpnnt/streamdeck-toolkit/commit/1167f366b9709b35442595e2c1fc8ba7df67a576) - **warnings**: address compiler warnings *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`393ef00`](https://github.com/cmpnnt/streamdeck-toolkit/commit/393ef00d93a3890e648a30dce3a62b2226d05970) - **docs**: remove old documentation and CI configuration files *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`ce54ddf`](https://github.com/cmpnnt/streamdeck-toolkit/commit/ce54ddf55d7571cdd9edff6eef92e6e56a1a404c) - **issues**: update issue template *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`4f3293d`](https://github.com/cmpnnt/streamdeck-toolkit/commit/4f3293d03eeaf22ed176912b0e61457b788acf23) - **sample plugin**: remove MacMinVersion *(commit by [@cmpnnt](https://github.com/cmpnnt))*

[v1.0.0]: https://github.com/cmpnnt/streamdeck-toolkit/compare/v0.1.0...v1.0.0
