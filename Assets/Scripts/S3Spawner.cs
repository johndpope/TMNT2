﻿using UnityEngine;
using System.Collections;

public class S3Spawner : MonoBehaviour {
	public GameObject PurpleFS;
	public GameObject StarFS;
	public GameObject[] pfs;
	public GameObject[] sfs;
	public bool go = false;
	public int goTime = 0;
	public MainCameraScript cams;
	public int atter = 1;
	public float screenstop = 577f;
	public float right;
	public float left;
	public int stage = 0;
	public Animator dooranim;
	public GameObject pfskickA_1;

	// Use this for initialization
	void Start () {
		cams = GameObject.Find("Main Camera").GetComponent<MainCameraScript>();
		sfs = new GameObject[5];
		right = screenstop + 32f;
		left = screenstop - (32f + 256f);
		dooranim = GameObject.Find("DoorA_1").GetComponent<Animator>();
		pfskickA_1 = GameObject.Find("pfskickA_1");
	}
	
	// Update is called once per frame
	void Update () {
		if(!go && dooranim.GetCurrentAnimatorStateInfo(0).IsName("DoorAEnd")){
			go = true;
			Destroy(pfskickA_1);
			spawnSFS(1, new Vector3(432f,114f,114f), 3);
			stage = 1;
		}
		if(go){
			if(goTime == 50){
				spawnSFS(2, new Vector3(left, 80f, 80f), 1);
				stage = 2;
			}else if(goTime == 100){
				spawnSFS (3, new Vector3(right, 40f, 40f), 2);
				stage = 3;
			}else if(goTime == 150){
				spawnSFS(4, new Vector3(right, 105f, 105f), 2);
				stage = 4;
				go = false;
			}
			goTime++;
		}
	}
	void spawnPFS(int num, Vector3 vec, int source){
		pfs[num] = Instantiate(PurpleFS, vec, Quaternion.identity) as GameObject;
		PurpleFSScript pfsscript = pfs[num].GetComponent<PurpleFSScript>();
		pfsscript.source = source;
		if (num == atter){
			pfsscript.attacker = true;
		}
		pfsscript.spawner = this;
		pfsscript.num = num;
		cams.enemiesAlive += 1;
	}
	void PFSdied(int num){
		Destroy(pfs[num]);
		cams.enemiesAlive -= 1;
		cams.points += 1;
		pfs[num] = null;
		if(atter == num && cams.enemiesAlive > 0){
			bool going = true;
			while(going){
				num++;
				if (pfs[num] != null){
					PurpleFSScript npscript = pfs[num].GetComponent<PurpleFSScript>();
					npscript.attacker = true;
					atter = num;
					going = false;
				}
			}
		}else if (stage != 4) {
			atter = stage + 1;
		}
	}
	void spawnSFS(int num, Vector3 vec, int source){
		sfs[num] = Instantiate(StarFS, vec, Quaternion.identity) as GameObject;
		StarFSScript sfsscript = sfs[num].GetComponent<StarFSScript>();
		sfsscript.source = source;
		sfsscript.screenstop = screenstop;
		if (num == atter){
			sfsscript.attacker = true;
		}
		sfsscript.spawner = this;
		sfsscript.num = num;
		cams.enemiesAlive += 1;
	}
	void SFSdied(int num){
		Destroy(sfs[num]);
		cams.enemiesAlive -= 1;
		cams.points += 1;
		sfs[num] = null;
		if(atter == num && cams.enemiesAlive > 0){
			bool going = true;
			while(going){
				num++;
				if (sfs[num] != null){
					StarFSScript nsscript = sfs[num].GetComponent<StarFSScript>();
					nsscript.attacker = true;
					atter = num;
					going = false;
				}
			}
		}else if (stage != 4) {
			atter = stage + 1;
		}
	}
}
