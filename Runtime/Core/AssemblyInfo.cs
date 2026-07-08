using System.Runtime.CompilerServices;

// Grants write access to ReferenceLocator's internal Register/Clear members.
// EnriRanjan.Core.Unity's ApplicationBootstrap is the only production writer;
// EnriRanjan.Core.Tests needs it to exercise ReferenceLocator directly.
[assembly: InternalsVisibleTo("EnriRanjan.Core.Unity")]
[assembly: InternalsVisibleTo("EnriRanjan.Core.Tests")]
