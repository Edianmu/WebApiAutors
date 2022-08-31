namespace WebApiAutors.Services
{
    public class WriteInFile : IHostedService
    {
        private readonly IWebHostEnvironment env;
        private readonly string fileName = "file 1.txt";
        private Timer timer;

        public WriteInFile(IWebHostEnvironment env)
        {
            this.env = env;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            Write("Proceso iniciado");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Dispose();
            Write("Proceso finalizado");
            return Task.CompletedTask;
        }

        private void DoWork (object state)
        {
            Write("Proceso en ejecución: " + DateTime.Now.ToString("dd/MM/yyyy hh:mm:ss"));
        }

        private void Write (string message)
        {
            var route = $@"{env.ContentRootPath}\wwwroot\{fileName}";
            using (StreamWriter writer = new StreamWriter(route, append: true))
            {
                writer.WriteLine (message);
            }
        }
    }
}
