namespace ProPresenter7WEB.Core
{
    public class PlaylistDetails
    {
        public required string Uuid { get; set; }

        public required string Name { get; set; }

        public required IEnumerable<PlaylistDetailsPresentation> Presentations { get; set; }
    }
}
