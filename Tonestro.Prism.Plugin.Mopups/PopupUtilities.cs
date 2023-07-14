using Mopups.Interfaces;
using Prism.Common;
using Tonestro.Prism.Plugin.Mopups.Extensions;
using Tonestro.Prism.Plugin.Mopups.Dialogs;
using NavigationMode = Prism.Navigation.NavigationMode;

namespace Tonestro.Prism.Plugin.Mopups;

internal static class PopupUtilities
    {
        public const string NavigationModeKey = "__NavigationMode";

        public static INavigationParameters CreateNewNavigationParameters() =>
            new NavigationParameters().AddNavigationMode(NavigationMode.New);

        public static INavigationParameters CreateBackNavigationParameters() =>
            new NavigationParameters().AddNavigationMode(NavigationMode.Back);

        public static INavigationParameters AddNavigationMode(this INavigationParameters parameters, NavigationMode mode)
        {
            return parameters.AddInternalParameter(NavigationModeKey, mode);
        }

        public static INavigationParameters AddInternalParameter(this INavigationParameters parameters, string key, object value)
        {
            ((INavigationParametersInternal)parameters).Add(key, value);
            return parameters;
        }

        public static Page TopPage(IPopupNavigation popupNavigation, IPageAccessor pageAccessor)
        {
            Page page;
            var popupStack = popupNavigation.PopupStack.Where(x => !(x is PopupDialogContainer));
            var popupPages = popupStack.ToList();
            if (popupPages.Any())
                page = popupPages.LastOrDefault();
            else if (pageAccessor.Page.Navigation.ModalStack.Count > 0)
                page = pageAccessor.Page.Navigation.ModalStack.LastOrDefault();
            else
                page =  pageAccessor.Page.Navigation.NavigationStack.LastOrDefault();

            page ??= pageAccessor.Page;

            return page.GetDisplayedPage();
        }

        public static Page GetOnNavigatedToTarget(IPopupNavigation popupNavigation, IPageAccessor pageAccessor)
        {
            Page page;
            if (popupNavigation.PopupStack.Count > 1)
                page = popupNavigation.PopupStack.ElementAt(popupNavigation.PopupStack.Count() - 2);
            else if (pageAccessor.Page.Navigation.ModalStack.Count > 0)
                page = pageAccessor.Page.Navigation.ModalStack.LastOrDefault();
            else
                page = pageAccessor.Page.Navigation.NavigationStack.LastOrDefault();

            page ??= pageAccessor.Page;

            return page.GetDisplayedPage();
        }
    }