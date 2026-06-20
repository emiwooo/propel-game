# scripts to implement!!!
player info
game manager - keep game state here
player controller

collectible (parent class for collectibles)
	small candy - to fill thrust meter
	large candy - gives large speed boost + make player immune for short time
	health - maybe? depends how hard game is
	gun - shoots bullets to kill birds

enemy (parent class probably) - damage player when they collide
	bird - can be shot down by bullets
	plane ? idk 

level generator - procedurally instantiate collectibles and enemies

camera controller - follow player? 

NICE TO HAVE - object recycler that reuses objects instead of constantly creating and destroying them