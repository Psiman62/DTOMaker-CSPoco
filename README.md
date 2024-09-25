# DTOMaker.MessagePack

*Warning: This is pre-release software under active development. Not for production use. Do not use if you can't tolerate breaking changes occasionally.*

Source generator for MessagePack data transport objects (DTOs).

## Quick Start
To use this generator:
1. create a new C# library project to contain your DTOs;
2. add a package reference to DTOMaker.Core;
3. define your models in C# as interfaces with simple properties;
4. add a reference to the MessagePack package;
5. add a reference to this package;
6. build your project;
7. voila! your generated MessagePack DTOs are ready to use.
