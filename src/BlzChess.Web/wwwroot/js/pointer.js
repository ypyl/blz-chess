// The little the browser has to answer for the drag. Everything else (which square is
// under the pointer, whether a release is a move) is C# in BlzChess.Core.
window.blzPointer = {
  // The board's box in CSS pixels. Pointer coordinates and the board are measured in
  // the same units, so the two can be subtracted without a scale factor.
  rect: (element) => {
    const box = element.getBoundingClientRect();
    return { left: box.left, top: box.top, width: box.width, height: box.height };
  },

  // Keeps the events coming to the board even when the pointer leaves it. Without this a
  // drag that wanders off the board never delivers its release, and the piece stays
  // lifted until the next press.
  capture: (element, pointerId) => {
    try {
      element.setPointerCapture(pointerId);
    } catch {
      // A pointer that is already gone cannot be captured, and the drag ends on its own.
    }
  },

  release: (element, pointerId) => {
    try {
      if (element.hasPointerCapture(pointerId)) {
        element.releasePointerCapture(pointerId);
      }
    } catch {
      // Same: nothing to release, nothing to do.
    }
  },
};
