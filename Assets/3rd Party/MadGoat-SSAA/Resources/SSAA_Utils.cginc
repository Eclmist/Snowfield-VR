
// include the unityCG.cginc
#include "UnityCG.cginc"

// The default v2f struct used in all the shaders
struct v2f {
	float4 vertex : SV_POSITION;
	float2 texcoord : TEXCOORD0;
};

/******************************
 ***	Bicubic helpers		***
 ******************************/

// amplitude functions
float ampl0(float a)
{
	return (1.0 / 6.0)*(a*(a*(-a + 3.0) - 3.0) + 1.0) + (1.0 / 6.0)*(a*a*(3.0*a - 6.0) + 4.0);
}

float ampl1(float a)
{
	return (1.0 / 6.0)*(a*(a*(-3.0*a + 3.0) + 3.0) + 1.0) + (1.0 / 6.0)*(a*a*a);
}

// offset functions
float off0(float a)
{
	return -1.0 + (1.0 / 6.0)*(a*a*(3.0*a - 6.0) + 4.0) / ((1.0 / 6.0)*(a*(a*(-a + 3.0) - 3.0) + 1.0) + (1.0 / 6.0)*(a*a*(3.0*a - 6.0) + 4.0));
}

float off1(float a)
{
	return 1.0 + (1.0 / 6.0)*(a*a*a) / ((1.0 / 6.0)*(a*(a*(-3.0*a + 3.0) + 3.0) + 1.0) + (1.0 / 6.0)*(a*a*a));
}
