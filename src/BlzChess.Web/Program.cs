using BlzChess.Core;
using BlzChess.Web;
using BlzChess.Web.Engine;

using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// One engine for the tab: the worker survives a new game, so switching levels does
// not download the wasm again.
builder.Services.AddSingleton<IEngine, StockfishEngine>();

await builder.Build().RunAsync();