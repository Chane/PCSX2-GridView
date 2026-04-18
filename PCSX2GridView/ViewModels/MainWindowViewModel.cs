namespace PCSX2GridView.ViewModels
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Linq;
    using System.Reactive.Concurrency;
    using System.Threading.Tasks;
    using Microsoft.Extensions.FileProviders;
    using PCSX2GridView.Backend;
    using ReactiveUI;

    public class MainWindowViewModel : ViewModelBase
    {
        private IApplicationAssetService assetService = null!;

        public MainWindowViewModel()
        {
            var rootCoverPath = Path.Combine(this.RootFilePath, "Covers");
            this.Initialize(
                new GameMediaService(
                    new PhysicalFileProvider(this.RootFilePath)),
                new CoverArtService(
                    new PhysicalFileProvider(rootCoverPath)));
            RxApp.MainThreadScheduler.Schedule(this.LoadLibrary);
        }

        public MainWindowViewModel(IGameMediaService gameMediaService, ICoverArtService artService)
        {
            this.Initialize(gameMediaService, artService);
            RxApp.MainThreadScheduler.Schedule(this.LoadLibrary);
        }

        public ObservableCollection<LibraryItemViewModel> Games { get; } = new ();

        private string RootFilePath =>
            Environment.GetEnvironmentVariable("PCSX2_GRIDVIEW_LIBRARY_PATH")
            ?? Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "Games",
                "ROMs",
                "PlayStation2");

        public void OnClickCommand()
        {
            this.LoadLibrary();
        }

        private void Initialize(IGameMediaService gameMediaService, ICoverArtService artService)
        {
            this.assetService = new ApplicationAssetService(gameMediaService, artService);
        }

        private async void LoadLibrary()
        {
            var items = await this.LoadGames().ConfigureAwait(false);

            this.Games.Clear();
            foreach (var item in items)
            {
                this.Games.Add(item);
            }

            this.LoadCovers();
        }

        private async Task<IEnumerable<LibraryItemViewModel>> LoadGames()
        {
            var result = new List<LibraryItemViewModel>();

            var games = await this.assetService.Fetch().ConfigureAwait(false);

            foreach (var game in games)
            {
                var item = new LibraryItemViewModel
                {
                    GameName = game.FileName,
                    PhysicalPath = Path.Combine(this.RootFilePath, game.FileName),
                    CoverArt = game.CoverArt,
                    AssetService = this.assetService,
                };

                result.Add(item);
            }

            return result.OrderBy(g => g.GameName);
        }

        private async void LoadCovers()
        {
            foreach (var gameViewModel in this.Games.ToList())
            {
                await gameViewModel.LoadCoverAsync();
            }
        }
    }
}