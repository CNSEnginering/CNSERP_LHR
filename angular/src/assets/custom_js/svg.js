// ==========================================
// ! (function) => img2svg
// ==========================================
	
	((window, { implementation }) => {
		const isLocal = window.location.protocol === "file:";
		const hasSvgSupport = implementation.hasFeature(
			"http://www.w3.org/TR/SVG11/feature#BasicStructure",
			"1.1"
		);
	
		function uniqueClasses(list) {
			list = list.split(" ");
			const hash = {};
			let i = list.length;
			const out = [];
			while (i--) {
				if (!hash.hasOwnProperty(list[i])) {
					hash[list[i]] = 1;
					out.unshift(list[i]);
				}
			}
			return out.join(" ");
		}
	
		const forEach =
			Array.prototype.forEach ||
			function (fn, scope) {
				if (this === void 0 || this === null || typeof fn !== "function") {
					throw new TypeError();
				}
	
				let i;
				const len = this.length >>> 0;
				for (i = 0; i < len; ++i) {
					if (i in this) {
						fn.call(scope, this[i], i, this);
					}
				}
			};
	
		const svgCache = {};
		let svgCount = 0;
		const svgCountEls = [];
		const requestQueue = [];
		const ranScripts = {};
		const svgClone = (sourceSvg) => sourceSvg.cloneNode(true);
		const queueRequest = (url, callback) => {
			requestQueue[url] = requestQueue[url] || [];
			requestQueue[url].push(callback);
		};
		const processRequestQueue = (url) => {
			for (let i = 0, len = requestQueue[url].length; i < len; i++) {
				((index) => {
					setTimeout(() => {
						requestQueue[url][index](svgClone(svgCache[url]));
					}, 0);
				})(i);
			}
		};
		const svgLoad = (url, callback) => {
			if (!window.SVGSVGElement) return;
			if (svgCache[url] !== undefined) {
				if (svgCache[url] instanceof SVGSVGElement) {
					callback(svgClone(svgCache[url]));
				} else {
					queueRequest(url, callback);
				}
			} else {
				if (!window.XMLHttpRequest) {
					callback("Browser does not support XMLHttpRequest");
					return false;
				}
				svgCache[url] = {};
				queueRequest(url, callback);
				const httpRequest = new XMLHttpRequest();
				httpRequest.onreadystatechange = () => {
					if (httpRequest.readyState === 4) {
						if (httpRequest.status === 404 || httpRequest.responseXML === null) {
							callback(`Unable to load SVG file: ${url}`);
							if (isLocal)
								callback(
									"Note: SVG injection ajax calls do not work locally without adjusting security setting in your browser. Or consider using a local webserver."
								);
							callback();
							return false;
						}
						if (httpRequest.status === 200 || (isLocal && httpRequest.status === 0)) {
							if (httpRequest.responseXML instanceof Document) {
								svgCache[url] = httpRequest.responseXML.documentElement;
							} else if (DOMParser && DOMParser instanceof Function) {
								let xmlDoc;
								try {
									const parser = new DOMParser();
									xmlDoc = parser.parseFromString(httpRequest.responseText, "text/xml");
								} catch (e) {
									xmlDoc = undefined;
								}
								if (!xmlDoc || xmlDoc.getElementsByTagName("parsererror").length) {
									callback(`Unable to parse SVG file: ${url}`);
									return false;
								} else {
									svgCache[url] = xmlDoc.documentElement;
								}
							}
							processRequestQueue(url);
						} else {
							callback(
								`There was a problem injecting the SVG: ${httpRequest.status} ${httpRequest.statusText}`
							);
							return false;
						}
					}
				};
				httpRequest.open("GET", url);
				if (httpRequest.overrideMimeType) httpRequest.overrideMimeType("text/xml");
				httpRequest.send();
			}
		};
		const injectElement = (el, evalScripts, pngFallback, callback) => {
			const imgUrl = el.getAttribute("data-src") || el.getAttribute("src");
			if (!/\.svg/i.test(imgUrl)) {
				callback(`Attempted to inject a file with a non-svg extension: ${imgUrl}`);
				return;
			}
			if (!hasSvgSupport) {
				const perElementFallback =
					el.getAttribute("data-fallback") || el.getAttribute("data-png");
				if (perElementFallback) {
					el.setAttribute("src", perElementFallback);
					callback(null);
				} else if (pngFallback) {
					el.setAttribute(
						"src",
						`${pngFallback}/${imgUrl.split("/").pop().replace(".svg", ".png")}`
					);
					callback(null);
				} else {
					callback(
						"This browser does not support SVG and no PNG fallback was defined."
					);
				}
				return;
			}
			if (svgCountEls.includes(el)) {
				return;
			}
	
			svgCountEls.push(el);
			el.setAttribute("src", "");
			svgLoad(imgUrl, (svg) => {
				if (typeof svg === "undefined" || typeof svg === "string") {
					callback(svg);
					return false;
				}
				const imgId = el.getAttribute("id");
				if (imgId) {
					svg.setAttribute("id", imgId);
				}
				const imgTitle = el.getAttribute("title");
				if (imgTitle) {
					svg.setAttribute("title", imgTitle);
				}
				const classMerge = []
					.concat(
						svg.getAttribute("class") || [],
						"injected-svg",
						el.getAttribute("class") || []
					)
					.join(" ");
				svg.setAttribute("class", uniqueClasses(classMerge));
				const imgStyle = el.getAttribute("style");
				if (imgStyle) {
					svg.setAttribute("style", imgStyle);
				}
				const imgData = [].filter.call(
					el.attributes,
					({ name }) => /^data-\w[\w\-]*$/.test(name) || "onclick".match(name)
				);
				forEach.call(imgData, ({ name, value }) => {
					if (name && value) {
						svg.setAttribute(name, value);
					}
				});
	
				const iriElementsAndProperties = {
					clipPath: ["clip-path"],
					"color-profile": ["color-profile"],
					cursor: ["cursor"],
					filter: ["filter"],
					linearGradient: ["fill", "stroke"],
					marker: ["marker", "marker-start", "marker-mid", "marker-end"],
					mask: ["mask"],
					pattern: ["fill", "stroke"],
					radialGradient: ["fill", "stroke"]
				};
				let element;
				let elementDefs;
				let properties;
				let currentId;
				let newId;
				Object.keys(iriElementsAndProperties).forEach((key) => {
					element = key;
					properties = iriElementsAndProperties[key];
					elementDefs = svg.querySelectorAll(`defs ${element}[id]`);
					for (let i = 0, elementsLen = elementDefs.length; i < elementsLen; i++) {
						currentId = elementDefs[i].id;
						newId = `${currentId}-${svgCount}`;
						let referencingElements;
						forEach.call(properties, (property) => {
							referencingElements = svg.querySelectorAll(
								`[${property}*="${currentId}"]`
							);
							for (
								let j = 0, referencingElementLen = referencingElements.length;
								j < referencingElementLen;
								j++
							) {
								referencingElements[j].setAttribute(property, `url(#${newId})`);
							}
						});
						elementDefs[i].id = newId;
					}
				});
				svg.removeAttribute("xmlns:a");
				const scripts = svg.querySelectorAll("script");
				const scriptsToEval = [];
				let script;
				let scriptType;
				for (let k = 0, scriptsLen = scripts.length; k < scriptsLen; k++) {
					scriptType = scripts[k].getAttribute("type");
					if (
						!scriptType ||
						scriptType === "application/ecmascript" ||
						scriptType === "application/javascript"
					) {
						script = scripts[k].innerText || scripts[k].textContent;
						scriptsToEval.push(script);
						svg.removeChild(scripts[k]);
					}
				}
				if (
					scriptsToEval.length > 0 &&
					(evalScripts === "always" ||
						(evalScripts === "once" && !ranScripts[imgUrl]))
				) {
					for (
						let l = 0, scriptsToEvalLen = scriptsToEval.length;
						l < scriptsToEvalLen;
						l++
					) {
						new Function(scriptsToEval[l])(window);
					}
					ranScripts[imgUrl] = true;
				}
				const styleTags = svg.querySelectorAll("style");
				forEach.call(styleTags, (styleTag) => {
					styleTag.textContent += "";
				});
				el.parentNode.replaceChild(svg, el);
				delete svgCountEls[svgCountEls.indexOf(el)];
				el = null;
				svgCount++;
				callback(svg);
			});
		};
	
		const img2svg = (elements, options = {}, done) => {
			const evalScripts = options.evalScripts || "always";
			const pngFallback = options.pngFallback || false;
			const eachCallback = options.each;
			if (elements && elements.length !== undefined) {
				let elementsLoaded = 0;
				forEach.call(elements, (element) => {
					injectElement(element, evalScripts, pngFallback, (svg) => {
						if (eachCallback && typeof eachCallback === "function") eachCallback(svg);
						if (done && elements.length === ++elementsLoaded) done(elementsLoaded);
					});
				});
			} else {
				if (elements) {
					injectElement(elements, evalScripts, pngFallback, (svg) => {
						if (eachCallback && typeof eachCallback === "function") eachCallback(svg);
						if (done) done(1);
						elements = null;
					});
				} else {
					if (done) done(0);
				}
			}
		};
		if (typeof module === "object" && typeof module.exports === "object") {
			module.exports = exports = img2svg;
		} else if (typeof define === "function" && define.amd) {
			define(() => img2svg);
		} else if (typeof window === "object") {
			window.img2svg = img2svg;
		}
	})(window, document);

	
