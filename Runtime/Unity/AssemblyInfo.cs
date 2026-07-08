using System.Runtime.CompilerServices;

// Grants EnriRanjan.Core.Editor.Tests access to ApplicationBootstrap's
// internal Locator property, needed to assert on registrations in tests.
[assembly: InternalsVisibleTo("EnriRanjan.Core.Editor.Tests")]
