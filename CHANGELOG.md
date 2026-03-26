

## [v2.0.0] - 2026-03-26
### :boom: BREAKING CHANGES
- due to [`e3aa29c`](https://github.com/cmpnnt/streamdeck-toolkit/commit/e3aa29c9d32ca649bffed04a1e0743a0d809dfbb) - simplify event routing and fix AOT-safe BaseEvent deserialization *(commit by [@cmpnnt](https://github.com/cmpnnt))*:

  plugin actions must implement new interface methods instead of subscribing to SdConnection events


### :sparkles: New Features
- [`8cd2612`](https://github.com/cmpnnt/streamdeck-toolkit/commit/8cd26125ffc5a04bf88ac581e3c056ef919964be) - implement source-generated AOT-safe settings population *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`73fe8b7`](https://github.com/cmpnnt/streamdeck-toolkit/commit/73fe8b7c4abd9d71f1202ee3eb8b9d4af0405573) - **manifest**: implement automatic manifest.json generation from source *(commit by [@cmpnnt](https://github.com/cmpnnt))*

### :bug: Bug Fixes
- [`c7584d6`](https://github.com/cmpnnt/streamdeck-toolkit/commit/c7584d6067c743c66c88e99ac00f0a9f6edc57ac) - **build**: replace Environment.OSVersion with RuntimeInformation for macOS compatibility *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`0613eb7`](https://github.com/cmpnnt/streamdeck-toolkit/commit/0613eb72f80eebde35aea3b5334dea886f24d954) - **manifest**: correctly emit codePathMac *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`11f3b1d`](https://github.com/cmpnnt/streamdeck-toolkit/commit/11f3b1dd9b8b2ac8b6615b5230befaebc0b4c127) - **AOT**: upgrade to NLog 6.x *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`ad511d4`](https://github.com/cmpnnt/streamdeck-toolkit/commit/ad511d4b01d0f0c94da69f062d6d61b0aab8b54a) - **build**: fix compiled binary name casing *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`b0576d7`](https://github.com/cmpnnt/streamdeck-toolkit/commit/b0576d7e636ea5f360696a5eb1d2a3f0195c737b) - **build**: fix publish/package plugin msbuild *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`9a9c542`](https://github.com/cmpnnt/streamdeck-toolkit/commit/9a9c542afb30df8163628ae3b38d73b233450fd1) - **AOT**: remove CommandLineParser package *(commit by [@cmpnnt](https://github.com/cmpnnt))*

### :recycle: Refactors
- [`e3aa29c`](https://github.com/cmpnnt/streamdeck-toolkit/commit/e3aa29c9d32ca649bffed04a1e0743a0d809dfbb) - simplify event routing and fix AOT-safe BaseEvent deserialization *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`de7fbb5`](https://github.com/cmpnnt/streamdeck-toolkit/commit/de7fbb5923bdf95d7ea00d4af5d1370f6a007eef) - **PluginContainer**: Extract common functionality in PluginContainer.cs for better readability *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`fbfc1c0`](https://github.com/cmpnnt/streamdeck-toolkit/commit/fbfc1c00e9317d691502967f6aa70dfdcc9e99ff) - **build**: move namespaces and upgrade framework/language versions *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`86c8723`](https://github.com/cmpnnt/streamdeck-toolkit/commit/86c87235eb21440aa304bcf6e2876a36a50155e6) - reorganize Cmpnnt.StreamDeckToolkit directory structure *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`2a323be`](https://github.com/cmpnnt/streamdeck-toolkit/commit/2a323beaf945d261ae3c02ac82afd795aa5aa684) - **naming**: rename SdWrapper to Toolkit *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`c2d5682`](https://github.com/cmpnnt/streamdeck-toolkit/commit/c2d5682ce7bdc35829ac65b9946bb11e58b4b082) - **solution**: rename slnx to match project name *(commit by [@cmpnnt](https://github.com/cmpnnt))*

### :white_check_mark: Tests
- [`cf9a452`](https://github.com/cmpnnt/streamdeck-toolkit/commit/cf9a4524f34c3d29e4e88ec8235dee15ce020197) - add event tests and move to new class *(commit by [@cmpnnt](https://github.com/cmpnnt))*

### :wrench: Chores
- [`1167f36`](https://github.com/cmpnnt/streamdeck-toolkit/commit/1167f366b9709b35442595e2c1fc8ba7df67a576) - **warnings**: address compiler warnings *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`393ef00`](https://github.com/cmpnnt/streamdeck-toolkit/commit/393ef00d93a3890e648a30dce3a62b2226d05970) - **docs**: remove old documentation and CI configuration files *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`ce54ddf`](https://github.com/cmpnnt/streamdeck-toolkit/commit/ce54ddf55d7571cdd9edff6eef92e6e56a1a404c) - **issues**: update issue template *(commit by [@cmpnnt](https://github.com/cmpnnt))*
- [`4f3293d`](https://github.com/cmpnnt/streamdeck-toolkit/commit/4f3293d03eeaf22ed176912b0e61457b788acf23) - **sample plugin**: remove MacMinVersion *(commit by [@cmpnnt](https://github.com/cmpnnt))*

[v2.0.0]: https://github.com/cmpnnt/streamdeck-toolkit/compare/v1.1.4...v2.0.0
