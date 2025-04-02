using System.Linq.Expressions;
using System.Reflection;

namespace YTDL
{
    public partial class App : Application
    {
        public App()
        {
            try
            {
                InitializeComponent();
            }
            catch (TargetInvocationException ex)
            {
                Console.WriteLine($"Inner Exception: {ex.InnerException?.Message}");
                throw;
            }
        }
        
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }
    }
}