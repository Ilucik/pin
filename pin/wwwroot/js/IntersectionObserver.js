export function construct(options) {
	return new IntersectionObserver(entries => {
		entries.forEach(entry => {
			if (entry.isIntersecting) {
				console.log('intersect ' + entry.intersectionRatio);
				entry.target.dispatchEvent(new CustomEvent("intersectionchanged", {
					bubbles: true,
					detail: { ratio: entry.intersectionRatio }
				}));
			}
		});
	}, options);
}