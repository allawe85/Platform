export function initChartEvents(container, panElement, dotNetHelper) {
    if (!container || !(container instanceof Element)) {
        console.warn("initChartEvents: container is not a valid Element", container);
        return;
    }
    if (!panElement || !(panElement instanceof Element)) {
        console.warn("initChartEvents: panElement is not a valid Element", panElement);
        return;
    }

    let isDown = false;
    let startX;
    let startY;
    let currentX = 0;
    let currentY = 0;

    // Read initial transform if any
    try {
        const style = window.getComputedStyle(panElement);
        const transform = style.transform || 'none';
        // Use DOMMatrix if available (standard), fallback to WebKitCSSMatrix
        const matrix = (window.DOMMatrix)
            ? new DOMMatrix(transform)
            : new WebKitCSSMatrix(transform);

        currentX = matrix.m41;
        currentY = matrix.m42;
    } catch (err) {
        console.warn("initChartEvents: failed to parse transform, checking legacy/fallback", err);
        currentX = 0;
        currentY = 0;
    }

    const onMouseDown = (e) => {
        isDown = true;
        container.style.cursor = 'grabbing';
        container.style.userSelect = 'none';
        startX = e.pageX - currentX;
        startY = e.pageY - currentY;
    };

    const stopDragging = () => {
        isDown = false;
        container.style.cursor = 'grab';
        container.style.removeProperty('user-select');
    };

    const onMouseLeave = stopDragging;
    const onMouseUp = stopDragging;

    const onMouseMove = (e) => {
        if (!isDown) return;
        e.preventDefault(); // Prevent selection

        const x = e.pageX - startX;
        const y = e.pageY - startY;

        currentX = x;
        currentY = y;

        panElement.style.transform = `translate(${x}px, ${y}px)`;
    };

    const onWheel = (e) => {
        if (e.ctrlKey) {
            e.preventDefault();
            const delta = Math.sign(e.deltaY) * -1;
            dotNetHelper.invokeMethodAsync('OnZoom', delta)
                .catch(err => console.error("Error in OnZoom invoke:", err));
        }
    };

    container.addEventListener('mousedown', onMouseDown);
    container.addEventListener('mouseleave', onMouseLeave);
    container.addEventListener('mouseup', onMouseUp);
    container.addEventListener('mousemove', onMouseMove);
    container.addEventListener('wheel', onWheel, { passive: false });

    container.style.cursor = 'grab';

    container._chartCleanup = () => {
        container.removeEventListener('mousedown', onMouseDown);
        container.removeEventListener('mouseleave', onMouseLeave);
        container.removeEventListener('mouseup', onMouseUp);
        container.removeEventListener('mousemove', onMouseMove);
        container.removeEventListener('wheel', onWheel);
        delete container._chartCleanup;
    };
}

export function stopChartEvents(element) {
    if (element && element._chartCleanup) {
        element._chartCleanup();
    }
}
