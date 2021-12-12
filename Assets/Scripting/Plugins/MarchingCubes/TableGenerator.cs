using System.Collections.Generic;
using UnityEngine;

public class TableGenerator {

	public int rotX(int number) {
		for (int j = 0; j < 8; j++) {
			caseVertexTemp[j] = (number & (1 << j)) == (1 << j);
		}

		caseVertex[0] = caseVertexTemp[3];
		caseVertex[1] = caseVertexTemp[0];
		caseVertex[2] = caseVertexTemp[1];
		caseVertex[3] = caseVertexTemp[2];

		caseVertex[4] = caseVertexTemp[7];
		caseVertex[5] = caseVertexTemp[4];
		caseVertex[6] = caseVertexTemp[5];
		caseVertex[7] = caseVertexTemp[6];

		int flagIndex = 0;
		for (int i = 0; i < 8; i++) {
			if (caseVertex[i]) flagIndex |= 1 << i;
		}

		return flagIndex;
	}

	public int rotY(int number) {
		for (int j = 0; j < 8; j++) {
			caseVertexTemp[j] = (number & (1 << j)) == (1 << j);
		}

		caseVertex[3] = caseVertexTemp[0];
		caseVertex[7] = caseVertexTemp[3];
		caseVertex[4] = caseVertexTemp[7];
		caseVertex[0] = caseVertexTemp[4];

		caseVertex[2] = caseVertexTemp[1];
		caseVertex[6] = caseVertexTemp[2];
		caseVertex[5] = caseVertexTemp[6];
		caseVertex[1] = caseVertexTemp[5];

		int flagIndex = 0;
		for (int i = 0; i < 8; i++) {
			if (caseVertex[i]) flagIndex |= 1 << i;
		}

		return flagIndex;
	}

	public int ReverseX(int number) {
		for (int j = 0; j < 8; j++) {
			caseVertexTemp[j] = (number & (1 << j)) == (1 << j);
		}

		caseVertex[3] = caseVertexTemp[0];
		caseVertex[0] = caseVertexTemp[1];
		caseVertex[1] = caseVertexTemp[2];
		caseVertex[2] = caseVertexTemp[3];

		caseVertex[7] = caseVertexTemp[4];
		caseVertex[4] = caseVertexTemp[5];
		caseVertex[5] = caseVertexTemp[6];
		caseVertex[6] = caseVertexTemp[7];

		int flagIndex = 0;
		for (int i = 0; i < 8; i++) {
			if (caseVertex[i]) flagIndex |= 1 << i;
		}

		return flagIndex;
	}

	public int ReverseY(int number) {
		for (int j = 0; j < 8; j++) {
			caseVertexTemp[j] = (number & (1 << j)) == (1 << j);
		}

		caseVertex[0] = caseVertexTemp[3];
		caseVertex[3] = caseVertexTemp[7];
		caseVertex[7] = caseVertexTemp[4];
		caseVertex[4] = caseVertexTemp[0];

		caseVertex[1] = caseVertexTemp[2];
		caseVertex[2] = caseVertexTemp[6];
		caseVertex[6] = caseVertexTemp[5];
		caseVertex[5] = caseVertexTemp[1];

		int flagIndex = 0;
		for (int i = 0; i < 8; i++) {
			if (caseVertex[i]) flagIndex |= 1 << i;
		}

		return flagIndex;
	}
	
	public int[] rotListX = {
		3, //0,
		0, //1,
		1, //2,
	    3, //3,
		7, //4,
		4, //5,
		5, //6,
		6, //7,
		11,//8,
		8, //9,
		9, //10,
		10,//11
	};

	public int[] rotListY = {
		4, //0,
		9, //1,
		0, //2,
		8, //3,
		6, //4,
		10, //5,
		2, //6,
		11, //7,
		7, //8,
		5, //9,
		1, //10,
		3, //11
	};
	
	public int[] rotPtListX = {
		6, //0,
		3, //1,
		0, //2,
		7, //3,
		4, //4,
		1, //5,
		8, //6,
		5, //7,
		2, //8,
		15,//9,
		12,//10,
		9, //11,
		16,//12,
		13,//13,
		10,//14,
		17,//15,
		14,//16,
		11,//17,
		24,//18,
		21,//19,
		18,//20,
		25,//21,
		22,//22,
		19,//23,
		26,//24,
		23,//25,
		20,//26
	};
	
	public int[] rotPtListY = {
		18,//0,
		19,//1,
		20,//2,
		9, //3,
		10,//4,
		11,//5,
		0, //6,
		1, //7,
		2, //8,
		21,//9,
		22,//10,
		23,//11,
		12,//12,
		13,//13,
		14,//14,
		3, //15,
		4, //16,
		5, //17,
		24,//18,
		25,//19,
		26,//20,
		15,//21,
		16,//22,
		17,//23,
		6, //24,
		7, //25,
		8, //26
	};

	public List<int> caseIndex = new List<int>();
	public List<int> storedIndex = new List<int>();
	public List<int> rotateIndex = new List<int>();


	public bool[] caseVertex = new bool[8];
	public bool[] caseVertexTemp = new bool[8];

	public int GetOriginIndex(int flagIndex) {
		return storedIndex[flagIndex];
	}

	public int GetOriginRotation(int flagIndex) {
		return rotateIndex[flagIndex];
	}

	public int RotateFlag(int flag, int rot) {
		if (rot == 0) return flag;
		if (rot == 1) return rotX(flag);
		if (rot == 2) return rotX(rotX(flag));
		if (rot == 3) return rotX(rotX(rotX(flag)));
		
		if (rot == 4) return rotY(flag);
		if (rot == 5) return rotX(rotY(flag));
		if (rot == 6) return rotX(rotX(rotY(flag)));
		if (rot == 7) return rotX(rotX(rotX(rotY(flag))));
		
		if (rot == 8) return rotY(rotY(flag));
		if (rot == 9) return rotX(rotY(rotY(flag)));
		if (rot == 10) return rotX(rotX(rotY(rotY(flag))));
		if (rot == 11) return rotX(rotX(rotX(rotY(rotY(flag)))));
		
		if (rot == 12) return rotY(rotY(rotY(flag)));
		if (rot == 13) return rotX(rotY(rotY(rotY(flag))));
		if (rot == 14) return rotX(rotX(rotY(rotY(rotY(flag)))));
		if (rot == 15) return rotX(rotX(rotX(rotY(rotY(rotY(flag))))));
		
		if (rot == 16) return rotY(rotX(flag));
		if (rot == 17) return rotX(rotY(rotX(flag)));
		if (rot == 18) return rotX(rotX(rotY(rotX(flag))));
		if (rot == 19) return rotX(rotX(rotX(rotY(rotX(flag)))));
		
		if (rot == 20) return rotY(rotX(rotX(rotX(flag))));
		if (rot == 21) return rotX(rotY(rotX(rotX(rotX(flag)))));
		if (rot == 22) return rotX(rotX(rotY(rotX(rotX(rotX(flag))))));
		if (rot == 23) return rotX(rotX(rotX(rotY(rotX(rotX(rotX(flag)))))));
		
		return flag;
	}

	public int ReverseRotateFlag(int flag, int rot) {
		if (rot == 0) return flag;
		if (rot == 1) return ReverseX(flag);
		if (rot == 2) return ReverseX(ReverseX(flag));
		if (rot == 3) return ReverseX(ReverseX(ReverseX(flag)));
		
		if (rot == 4) return ReverseY(flag);
		if (rot == 5) return ReverseY(ReverseX(flag));
		if (rot == 6) return ReverseY(ReverseX(ReverseX(flag)));
		if (rot == 7) return ReverseY(ReverseX(ReverseX(ReverseX(flag))));
		
		if (rot == 8) return ReverseY(ReverseY(flag));
		if (rot == 9) return ReverseY(ReverseY(ReverseX(flag)));
		if (rot == 10) return ReverseY(ReverseY(ReverseX(ReverseX(flag))));
		if (rot == 11) return ReverseY(ReverseY(ReverseX(ReverseX(ReverseX(flag)))));
		
		if (rot == 12) return ReverseY(ReverseY(ReverseY(flag)));
		if (rot == 13) return ReverseY(ReverseY(ReverseY(ReverseX(flag))));
		if (rot == 14) return ReverseY(ReverseY(ReverseY(ReverseX(ReverseX(flag)))));
		if (rot == 15) return ReverseY(ReverseY(ReverseY(ReverseX(ReverseX(ReverseX(flag))))));
		
		if (rot == 16) return ReverseX(ReverseY(flag));
		if (rot == 17) return ReverseX(ReverseY(ReverseX(flag)));
		if (rot == 18) return ReverseX(ReverseY(ReverseX(ReverseX(flag))));
		if (rot == 19) return ReverseX(ReverseY(ReverseX(ReverseX(ReverseX(flag)))));
		
		if (rot == 20) return ReverseX(ReverseX(ReverseX(ReverseY(flag))));
		if (rot == 21) return ReverseX(ReverseX(ReverseX(ReverseY(ReverseX(flag)))));
		if (rot == 22) return ReverseX(ReverseX(ReverseX(ReverseY(ReverseX(ReverseX(flag))))));
		if (rot == 23) return ReverseX(ReverseX(ReverseX(ReverseY(ReverseX(ReverseX(ReverseX(flag)))))));
		return flag;
	}

	public int RotateCubePoint(int point, int rot) {
		if (point == -1) return -1;
		
		if (rot == 0) return point;
		if (rot == 1) return rotPtListX[point];
		if (rot == 2) return rotPtListX[rotPtListX[point]];
		if (rot == 3) return rotPtListX[rotPtListX[rotPtListX[point]]];
			
		if (rot == 4) return rotPtListY[point];
		if (rot == 5) return rotPtListY[rotPtListX[point]];
		if (rot == 6) return rotPtListY[rotPtListX[rotPtListX[point]]];
		if (rot == 7) return rotPtListY[rotPtListX[rotPtListX[rotPtListX[point]]]];
		
		if (rot == 8) return rotPtListY[rotPtListY[point]];
		if (rot == 9) return rotPtListY[rotPtListY[rotPtListX[point]]];
		if (rot == 10) return rotPtListY[rotPtListY[rotPtListX[rotPtListX[point]]]];
		if (rot == 11) return rotPtListY[rotPtListY[rotPtListX[rotPtListX[rotPtListX[point]]]]];
			
		if (rot == 12) return rotPtListY[rotPtListY[rotPtListY[point]]];
		if (rot == 13) return rotPtListY[rotPtListY[rotPtListY[rotPtListX[point]]]];
		if (rot == 14) return rotPtListY[rotPtListY[rotPtListY[rotPtListX[rotPtListX[point]]]]];
		if (rot == 15) return rotPtListY[rotPtListY[rotPtListY[rotPtListX[rotPtListX[rotPtListX[point]]]]]];
			
		if (rot == 16) return rotPtListX[rotPtListY[point]];
		if (rot == 17) return rotPtListX[rotPtListY[rotPtListX[point]]];
		if (rot == 18) return rotPtListX[rotPtListY[rotPtListX[rotPtListX[point]]]];
		if (rot == 19) return rotPtListX[rotPtListY[rotPtListX[rotPtListX[rotPtListX[point]]]]];

		if (rot == 20) return rotPtListX[rotPtListX[rotPtListX[rotPtListY[point]]]];
		if (rot == 21) return rotPtListX[rotPtListX[rotPtListX[rotPtListY[rotPtListX[point]]]]];
		if (rot == 22) return rotPtListX[rotPtListX[rotPtListX[rotPtListY[rotPtListX[rotPtListX[point]]]]]];
		if (rot == 23) return rotPtListX[rotPtListX[rotPtListX[rotPtListY[rotPtListX[rotPtListX[rotPtListX[point]]]]]]];

		return point;
	}

	public int RotateEdgePoint(int point, int rot) {
		if (point == -1) return -1;
		
		if (rot == 0) return point;
		if (rot == 1) return rotListX[point];
		if (rot == 2) return rotListX[rotListX[point]];
		if (rot == 3) return rotListX[rotListX[rotListX[point]]];
			
		if (rot == 4) return rotListY[point];
		if (rot == 5) return rotListY[rotListX[point]];
		if (rot == 6) return rotListY[rotListX[rotListX[point]]];
		if (rot == 7) return rotListY[rotListX[rotListX[rotListX[point]]]];
		
		if (rot == 8) return rotListY[rotListY[point]];
		if (rot == 9) return rotListY[rotListY[rotListX[point]]];
		if (rot == 10) return rotListY[rotListY[rotListX[rotListX[point]]]];
		if (rot == 11) return rotListY[rotListY[rotListX[rotListX[rotListX[point]]]]];
			
		if (rot == 12) return rotListY[rotListY[rotListY[point]]];
		if (rot == 13) return rotListY[rotListY[rotListY[rotListX[point]]]];
		if (rot == 14) return rotListY[rotListY[rotListY[rotListX[rotListX[point]]]]];
		if (rot == 15) return rotListY[rotListY[rotListY[rotListX[rotListX[rotListX[point]]]]]];
			
		if (rot == 16) return rotListY[rotListX[point]];
		if (rot == 17) return rotListX[rotListY[rotListX[point]]];
		if (rot == 18) return rotListX[rotListY[rotListX[rotListX[point]]]];
		if (rot == 19) return rotListX[rotListY[rotListX[rotListX[rotListX[point]]]]];

		if (rot == 20) return rotListX[rotListX[rotListX[rotListY[point]]]];
		if (rot == 21) return rotListX[rotListX[rotListX[rotListY[rotListX[point]]]]];
		if (rot == 22) return rotListX[rotListX[rotListX[rotListY[rotListX[rotListX[point]]]]]];
		if (rot == 23) return rotListX[rotListX[rotListX[rotListY[rotListX[rotListX[rotListX[point]]]]]]];
		
		return point;
	}

	public bool GetTriangleList(int caseId, int rot, int negFlagIndex, int[] triangles) {
		negFlagIndex = RotateFlag(negFlagIndex, rot) & caseFlags[caseId, 0];

		for (int i = 1; i < caseFlags.GetLength(1); i += 2) {
			if (caseFlags[caseId, i] == negFlagIndex) {
				int listId = caseFlags[caseId, i + 1];
				for (int j = 0; j < caseTriangles.GetLength(1); j++) {
					int p = caseTriangles[listId, j];
					if (p < 12) {
						triangles[j] = RotateEdgePoint(p, rot);
					} else {
						triangles[j] = RotateCubePoint(p - 12, rot) + 12;
					}
				}

				return true;
			}
		}

		return false;
	}

	public void AddStoreRot(int store, int rot) {
		if (store == 21) {
			storedIndex.Add(0);
			
		} else if (store == 16) {
			storedIndex.Add(1);
			
		} else if (store == 5) {
			storedIndex.Add(2);
			
		} else {
			storedIndex.Add(-1);
			
		}

		rotateIndex.Add(rot);
	}

	public void GetCases() {
		for (int i = 0; i < 256; i++) {
			int px0 = i;
			int px1 = rotX(px0);
			int px2 = rotX(px1);
			int px3 = rotX(px2);

			int py1x0 = rotY(px0);
			int py1x1 = rotX(py1x0);
			int py1x2 = rotX(py1x1);
			int py1x3 = rotX(py1x2);

			int py2x0 = rotY(py1x0);
			int py2x1 = rotX(py2x0);
			int py2x2 = rotX(py2x1);
			int py2x3 = rotX(py2x2);

			int py3x0 = rotY(py2x0);
			int py3x1 = rotX(py3x0);
			int py3x2 = rotX(py3x1);
			int py3x3 = rotX(py3x2);

			int pz1x0 = rotY(rotX(i));
			int pz1x1 = rotX(pz1x0);
			int pz1x2 = rotX(pz1x1);
			int pz1x3 = rotX(pz1x2);

			int pz2x0 = rotY(rotX(rotX(rotX(i))));
			int pz2x1 = rotX(pz2x0);
			int pz2x2 = rotX(pz2x1);
			int pz2x3 = rotX(pz2x2);

			int store = caseIndex.IndexOf(px0);
			if (store != -1) AddStoreRot(store, 0); // must be impossible
			else if ((store = caseIndex.IndexOf(px1)) != -1) AddStoreRot(store, 1);
			else if ((store = caseIndex.IndexOf(px2)) != -1) AddStoreRot(store, 2);
			else if ((store = caseIndex.IndexOf(px3)) != -1) AddStoreRot(store, 3);
			else if ((store = caseIndex.IndexOf(py1x0)) != -1) AddStoreRot(store, 4);
			else if ((store = caseIndex.IndexOf(py1x1)) != -1) AddStoreRot(store, 5);
			else if ((store = caseIndex.IndexOf(py1x2)) != -1) AddStoreRot(store, 6);
			else if ((store = caseIndex.IndexOf(py1x3)) != -1) AddStoreRot(store, 7);
			else if ((store = caseIndex.IndexOf(py2x0)) != -1) AddStoreRot(store, 8);
			else if ((store = caseIndex.IndexOf(py2x1)) != -1) AddStoreRot(store, 9);
			else if ((store = caseIndex.IndexOf(py2x2)) != -1) AddStoreRot(store, 10);
			else if ((store = caseIndex.IndexOf(py2x3)) != -1) AddStoreRot(store, 11);
			else if ((store = caseIndex.IndexOf(py3x0)) != -1) AddStoreRot(store, 12);
			else if ((store = caseIndex.IndexOf(py3x1)) != -1) AddStoreRot(store, 13);
			else if ((store = caseIndex.IndexOf(py3x2)) != -1) AddStoreRot(store, 14);
			else if ((store = caseIndex.IndexOf(py3x3)) != -1) AddStoreRot(store, 15);
			else if ((store = caseIndex.IndexOf(pz1x0)) != -1) AddStoreRot(store, 16);
			else if ((store = caseIndex.IndexOf(pz1x1)) != -1) AddStoreRot(store, 17);
			else if ((store = caseIndex.IndexOf(pz1x2)) != -1) AddStoreRot(store, 18);
			else if ((store = caseIndex.IndexOf(pz1x3)) != -1) AddStoreRot(store, 19);
			else if ((store = caseIndex.IndexOf(pz2x0)) != -1) AddStoreRot(store, 20);
			else if ((store = caseIndex.IndexOf(pz2x1)) != -1) AddStoreRot(store, 21);
			else if ((store = caseIndex.IndexOf(pz2x2)) != -1) AddStoreRot(store, 22);
			else if ((store = caseIndex.IndexOf(pz2x3)) != -1) AddStoreRot(store, 23);
			else {
				AddStoreRot(caseIndex.Count, 0);
				caseIndex.Add(i);
			}

		}
	}

	public int[,] caseFlags = {
		{
			0b01011000, 
			0b00001000,  1, 
			0b00010000,  2, 
			0b01000000,  3, 
			0b00011000,  4, 
			0b01001000,  5, 
			0b01010000,  6, 
			0b01011000,  0, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
		},
		{
			0b00111100, 
			0b00010000,  7, 
			0b00100000,  8, 
			0b00110000,  9,
			0b00001000, 10, 
			0b00000100, 11,
			0b00001100, 12, 
			0b00011000, 13, 
			0b00101000, 14, 
			0b00111000, 15, 
			0b00010100, 16, 
			0b00100100, 17, 
			0b00110100, 18, 
			0b00011100, 19, 
			0b00101100, 20, 
			0b00111100,  0
		},
		{
			0b11111111, 
			0b00000001, 21, 
			0b00000010, 22, 
			0b00000011, 23, 
			0b00000100, 24, 
			0b00000101, 25, 
			0b00000110, 26, 
			0b00000111, 27, 
			0b00001000, 28,  -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1
			
		},
	};

	// todo - remover o ponto 13, e usalo automaticamente em todos
	public int[,] caseTriangles = {
		{-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}, 
		{13+12, 6, 7, 13+12, 7, 12+12, 13+12, 16+12, 6, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}, 
		{13+12, 6, 22+12, 13+12, 11, 6, 13+12, 12+12, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}, 
		{13+12, 11, 16+12, 13+12, 7, 11, 13+12, 22+12, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
		{13+12, 6, 22+12, 13+12, 16+12, 6, -1, -1, -1, 1-1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}, 
		{13+12, 7, 12+12, 13+12, 22+12, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},  
		{13+12, 12+12, 11, 13+12, 11, 16+12, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
		// 7
		{13+12, 12+12, 11, 13+12, 11, 10, 13+12, 10,  5, 13+12, 5, 22+12, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
		{13+12, 7, 11, 13+12, 11, 10, 13+12, 10, 14+12, 13+12, 22+12, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
		{13+12, 12+12, 11, 13+12, 11, 10, 13+12, 10, 14+12, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
		{13+12, 7, 12+12, 13+12, 5, 7, 13+12, 10, 5, 13+12, 16+12, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
		{13+12, 7, 11, 13+12, 11, 16+12, 13+12, 5, 7, 13+12, 14+12, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
		{13+12, 7, 12+12, 13+12, 5, 7, 13+12, 14+12, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}, 
		{13+12, 16+12, 10, 13+12, 5, 22+12, 13+12, 10, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
		{13+12, 10, 14+12, 13+12, 16+12, 10, 13+12, 7, 12+12, 13+12, 22+12, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1,  -1, -1, -1},
		{13+12, 10, 14+12, 13+12, 16+12, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
		{13+12, 12+12, 11, 13+12, 11, 16+12, 13+12, 5, 22+12, 13+12, 14+12, 5, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
		{13+12, 22+12, 7, 13+12, 7, 11, 13+12, 11, 16+12, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}, 
		{13+12, 12+12, 11, 13+12, 11, 16+12, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
		{13+12, 14+12, 5, 13+12, 5, 22+12, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}, 
		{13+12, 7, 12+12, 13+12, 22+12, 7, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}, 
		// 21
		{13+12, 10, 9, 13+12, 9, 10+12, 13+12, 11, 10, 13+12, 12+12, 11, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}, 
		{13+12, 10+12, 8, 13+12, 8, 11, 13+12, 11, 10, 13+12, 10, 14+12, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}, 
		{13+12, 12+12, 11, 13+12, 11, 10, 13+12, 10, 14+12, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1},
		{13+12, 14+12, 9, 13+12, 9, 8, 13+12, 8, 11, 13+12, 11, 16+12, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}, 
		{13+12, 14+12, 9, 13+12, 9, 10+12, 13+12, 12+12, 11, 13+12, 11, 16+12, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}, 
		{13+12, 10+12, 8, 13+12, 8, 11, 13+12, 11, 16+12, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}, 
		{13+12, 12+12, 11, 13+12, 11, 16+12, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}, 
		{13+12, 8, 12+12, 13+12, 9, 8, 13+12, 10, 9, 13+12, 16+12, 10, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1}, 
	};
}