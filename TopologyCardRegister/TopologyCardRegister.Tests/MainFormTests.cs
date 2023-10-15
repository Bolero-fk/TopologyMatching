namespace TopologyCardRegister.Tests
{
    public class MainFormTests
    {
        [Fact]
        public void ConstructorTest()
        {
            var form = new MainForm();
        }

        [Fact]
        public void DisposeTest()
        {
            var form = new MainForm();
            form.Dispose();
        }
    }
}
