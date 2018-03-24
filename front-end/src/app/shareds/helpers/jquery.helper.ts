import { JsHelper } from './js.helper';

export class JQueryHelper {
    public static onClickOutside(
        target: string | any,
        func: any,
        excludeTargets: string[] = []): void {

        $(document).click((e: any) => {
            const container: JQuery = $(target);

            let inExcludeTarget: boolean = false;
            for (const val of excludeTargets) {
                if (container.is($(val))) {
                    inExcludeTarget = true;
                    break;
                }
            }

            if (!inExcludeTarget &&
                !container.is(e.target) &&
                container.has(e.target).length === 0) {
                func();
            }
        });
    }
    public static showLoading(selector: string = '.loading-page'): void {
        // console.log('About to show loading ...');
        $(selector).css('display', 'flex');
    }

    public static hideLoading(selector: string = '.loading-page'): void {
        // console.log('About to hide loading ...');
        $(selector).css('display', 'none');
    }

    public static showLocalLoading(selector: string = '.loading-local'): void {
        // console.log('About to show loading ...');
        $(selector).css('display', 'flex');
    }

    public static hideLocalLoading(selector: string = '.loading-local'): void {
        // console.log('About to hide loading ...');
        $(selector).css('display', 'none');
    }

    public static showOverlay(selector: string = '#overlay'): void {
        $(selector).fadeIn();
    }

    public static hideOverlay(selector: string = '#overlay'): void {
        $(selector).fadeOut();
    }
}
