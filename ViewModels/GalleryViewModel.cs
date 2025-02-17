using System.ComponentModel.DataAnnotations;

namespace LezzetKapinda.ViewModels;

public sealed class GalleryViewModel
{
    public IEnumerable<UserOrderImages> UserOrderImages { get; set; }
}