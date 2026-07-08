using NUnit.Framework;

namespace EnriRanjan.Core.Editor.Tests
{
    public class EditorPlaceholderTests
    {
        [Test]
        public void EditorPlaceholder_CanBeConstructed()
        {
            var instance = new EditorPlaceholder();

            Assert.IsNotNull(instance);
        }
    }
}
