function tabMenuClick(element)
{
	var activeTabs = document.getElementsByClassName("tab-active");
	var activeTabsCopy = [];
	for(i = 0; i < activeTabs.length; i++)
		activeTabsCopy.push(activeTabs[i]);
	
	for(i = 0; i < activeTabsCopy.length; i++)
	{
		var currentElement = activeTabsCopy[i];
		currentElement.classList.remove("tab-active");
		if (currentElement.className === "tab-content") 
		{
			currentElement.style.display = "none";
		}
	}

	var tabMenuChilds = document.getElementsByClassName("tab-menu")[0].childNodes;
	var active = [];
	for (i = 0; i < tabMenuChilds.length; i++)
		if (element.id === tabMenuChilds[i].id) active.push(tabMenuChilds[i]);
	for (i = 0; i < active.length; i++)
	{
		if (active[i].className === "tab-content") 
		{
			active[i].style.display = "block";
		}
		active[i].className += " tab-active";
	}
}

