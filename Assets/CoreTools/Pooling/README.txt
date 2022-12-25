Copyright (c) 2021 - Christoph Römer. All rights reserved. 

For support, feedback and suggestions please conact me under:
contactsundiray@gmail.com

Check out my other content:
https://sundiray.itch.io/

_________________________________________________________________________
 Version 1.0:
	Initial Release 26.04.2022

_________________________________________________________________________

>>> Getting started:
	At the top bar of the Unity Editor select Tools/Get PoolManager.
	This create a global poolmanager gameobject in
	the scene hierarchy if there is none already. In either case the global 
	poolmanager will be selected. In its Inspector you'll find a list called
	'Global Pools' where you can add or remove gameobject pools.
	To request a pooled object from a global pool use:
	'GlobalPoolManager.Instance.RequestObject(...)' the argument being 
	the key for the targeted pool.

>>> Local Pool Manager
	You can add a LocalPoolManager to a GameObjec if you want to seperate out
	pools and have them tied to a different gameobject. These local poolmanagers
	can only be accessed by having a direct reference to them.

>>> Local Pool Simple
	You can also only add a component that manages a single pool of gameobjects.
	You'll need a reference to access the pool and the pooled objects are 
	retrieved by calling ".GetPooledGameobject()"

>>> Scene Persistence
	The GlobalPoolManager is not persistent. If that is your goal consider either
	using a seperate additive and persistent scene and communicate the requests
	via an event system or alternatively you can change the 'Persitent' property at
	the top of the Pooling/Global/GlobalPoolManager cs file to 'true'.
	