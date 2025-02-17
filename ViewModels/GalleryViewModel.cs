using System.ComponentModel.DataAnnotations;
using LezzetKapinda.Configurations;
using UserOrderImages = LezzetKapinda.Models.UserOrderImages;

namespace LezzetKapinda.ViewModels;

public sealed class GalleryViewModel
{
    public List<UserOrderImages> UserOrderImages { get; set; }
}