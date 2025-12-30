export function initChartEvents(container, panElement, dotNetHelper) {
    if (!container || !panElement) return;

    let state = {
        isDown: false,
        startX: 0,
        startY: 0,
        currentX: 0,
        currentY: 0
    };

    // Read initial transform
    try {
        const style = window.getComputedStyle(panElement);
        const matrix = new (window.DOMMatrix || window.WebKitCSSMatrix)(style.transform || 'none');
        state.currentX = matrix.m41;
        state.currentY = matrix.m42;
    } catch (err) {
        state.currentX = 0;
        state.currentY = 0;
    }

    const updateTransform = () => {
        panElement.style.transform = `translate(${state.currentX}px, ${state.currentY}px)`;
    };

    const onMouseDown = (e) => {
        state.isDown = true;
        container.style.cursor = 'grabbing';
        state.startX = e.pageX - state.currentX;
        state.startY = e.pageY - state.currentY;
    };

    const stopDragging = () => {
        state.isDown = false;
        container.style.cursor = 'grab';
    };

    const onMouseMove = (e) => {
        if (!state.isDown) return;
        e.preventDefault();
        state.currentX = e.pageX - state.startX;
        state.currentY = e.pageY - state.startY;
        updateTransform();
    };

    const onWheel = (e) => {
        if (e.ctrlKey) {
            e.preventDefault();
            const delta = Math.sign(e.deltaY) * -1;
            dotNetHelper.invokeMethodAsync('OnZoom', delta);
        }
    };

    container.addEventListener('mousedown', onMouseDown);
    window.addEventListener('mouseup', stopDragging);
    window.addEventListener('mousemove', onMouseMove);
    container.addEventListener('wheel', onWheel, { passive: false });

    container._chartState = state;
    container._chartUpdate = updateTransform;
    container._chartCleanup = () => {
        container.removeEventListener('mousedown', onMouseDown);
        window.removeEventListener('mouseup', stopDragging);
        window.removeEventListener('mousemove', onMouseMove);
        container.removeEventListener('wheel', onWheel);
    };
}

export function centerElement(container, panElement, targetSelector) {
    if (!container || !panElement || !container._chartState) return;

    const targetNode = targetSelector ? panElement.querySelector(targetSelector) : panElement.querySelector('.node-card');
    if (!targetNode) return;

    // Get rects
    const containerRect = container.getBoundingClientRect();
    const targetRect = targetNode.getBoundingClientRect();

    // Calculate current center difference
    const containerCenterX = containerRect.left + containerRect.width / 2;
    const containerCenterY = containerRect.top + 200; // Prefer top-centering for trees

    const targetCenterX = targetRect.left + targetRect.width / 2;
    const targetCenterY = targetRect.top + targetRect.height / 2;

    const diffX = containerCenterX - targetCenterX;
    const diffY = containerCenterY - targetCenterY;

    // Apply new translation
    container._chartState.currentX += diffX;
    container._chartState.currentY += diffY;
    container._chartUpdate();
}

export function stopChartEvents(element) {
    if (element && element._chartCleanup) {
        element._chartCleanup();
    }
}
