Copyright (c) 2021 - Christoph Römer. All rights reserved. 

For support, feedback and suggestions please conact me under:
contactsundiray@gmail.com

Check out my other content:
https://sundiray.itch.io/

_________________________________________________________________________
 Version 1.0:
	

_________________________________________________________________________

>>> Getting started:
	At the top bar of the Unity Editor select Tools/Get PoolManager.
	This will either select or create a global poolmanager gameobject in
	the hierarchy. In its Inspector you'll find a list called
	'Global Pools'. All Pools you want to globally access via:
	'GlobalPoolManager.Instance.RequestObject(...)' the argument being 
	the key for targeted pool.

>>> Local Pool Manager
	

>>> Scene Persistence
	The GlobalPoolManager is not persistent. If that is your goal consider
	using a seperate additive and persistent scene and communicate the requests
	via an event system.
	