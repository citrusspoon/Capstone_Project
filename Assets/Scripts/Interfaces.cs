﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITurretInfo {

	TurretType GetType();
	void BoostFireRate(float boostAmount);
}
