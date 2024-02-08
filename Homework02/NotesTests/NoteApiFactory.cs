using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using MAUINotes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using NotesAPI.Initializers;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace NotesTests
{
    public class NoteApiFactory : WebApplicationFactory<Program>
    {
		public string DatabaseName { get; set; } = "";
		private static string DatabaseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
		protected override void ConfigureWebHost(IWebHostBuilder builder)
		{
			 DatabaseName = Guid.NewGuid().ToString();
			builder.ConfigureTestServices(services =>
			{
				services.RemoveAll(typeof(DatabaseInitialize));
				services.AddScoped<DatabaseInitialize>(x => new DatabaseInitialize(DatabaseName));
			});
		}

		public override ValueTask DisposeAsync()
		{
			File.Delete(Path.Combine(DatabaseDirectory, DatabaseName));
			return base.DisposeAsync();
		}

	}


}
