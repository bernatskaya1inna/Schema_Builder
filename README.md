## Schema_Builder
CHILI	Publisher	holds	a	concept	called	“folding	settings”.	These	settings	enable	users	to	render	a	document	in	3D,	so	the	end	user	can	see	how	his/her	design	folds.			

The	folding	configuration	is	stored	in	an	XML	file.		

The	assets	folder	of	this	challenge	contains	the	following	file:	Beerpack.xml.		

The main purpose is:
1. Parsing	the	supplied	XML	file	(Beerpack.xml)	and	loads	it	into	an	object	model.	

2. Rendering	a	2D	representation	of	the	folding	scheme.

There	is	always	one	root	panel,	and	each	panel	can	have	child	panels.	The	children	of	each	panel	can	be	found	in	the	“attachedPanels”	child	node	of	each	panel.

The	position	of	the	root	panel	is	stored	in	the	“rootX”	and	“rootY”	attributes	of	the	document	element.	

Each	panel	(except	for	the	root	panel)	is	attached	to	a	specific	side	of	its	parent	panel.	The	side	index	is	stored	in	the	“attachedToSide”	attribute	of	each	panel.

Each	attached	panel	is	rotated	depending	on	the	side	it	is	attached	to.	With	side	0	it’s	always	attached	to	a	side	of	its	parent	panel.

To	create	the	bitmap	you	can	use	System.Drawing.Bitmap.	This	class	can	easily	output	to	a	JPG	file.	
