using Mopups.Interfaces;
using Mopups.Pages;
using Prism.Behaviors;
using Prism.Common;

namespace Tonestro.Prism.Plugin.Mopups;

/// <summary>
/// Provides an implementation of the <see cref="IPageBehaviorFactory" /> to add the 
/// <see cref="BackgroundPopupDismissalBehavior" /> on <see cref="PopupPage" />.
/// </summary>
public class PopupPageBehaviorFactory : IPageBehaviorFactory
{
    IPopupNavigation PopupNavigation { get; }
    IPageAccessor PageAccessor { get; }

    /// <summary>
    /// Creates a new <see cref="IPageBehaviorFactory" /> for use with <see cref="PopupPage" />.
    /// </summary>
    /// <param name="popupNavigation">The <see cref="IPopupNavigation" /> service.</param>
    /// <param name="pageAccessor">The <see cref="IPageAccessor" />.</param>
    public PopupPageBehaviorFactory(IPopupNavigation popupNavigation, IPageAccessor pageAccessor)
    {
        PopupNavigation = popupNavigation;
        PageAccessor = pageAccessor;
    }

    /// <inheritdoc />
    public void ApplyPageBehaviors(Page page)
    {
        if (page is PopupPage popupPage)
        {
            popupPage.Behaviors.Add(new BackgroundPopupDismissalBehavior(PopupNavigation, PageAccessor));
        }
    }
}