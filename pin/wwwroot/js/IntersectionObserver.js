export function construct(options) {
	return new IntersectionObserver(entries => {
		entries.forEach(entry => {
			if (entry.isIntersecting) {
				entry.target.dispatchEvent(new CustomEvent("intersectionchanged", {
					bubbles: true,
					detail: { ratio: entry.intersectionRatio }
				}));
			}
		});
	}, options);
}