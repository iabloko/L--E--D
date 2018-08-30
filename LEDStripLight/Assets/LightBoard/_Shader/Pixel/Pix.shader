// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/pixelate" {
Properties{
_ResolutionX ("ResolutionX", int) = 100
_ResolutionY ("ResolutionY", int) = 100
_Samples ("Samples", int) = 5
_Radius ("LED Radius", float) = 1.0
_MainTex ("Main Texture", 2D) = "white" {}
_ChromaColor ("Chroma color", color) = (1.0, 1.0, 1.0)
_Feather ("Feather", Range(0, 1)) = 0.0
_Size ("Size", float) = 1.0
}
    SubShader {
        Pass {
        Tags { "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma target 3.0
           
            float _ResolutionX;
            float _ResolutionY;
            float _Samples;
            sampler2D _MainTex;
            float _Radius;
            fixed4 _ChromaColor;
            float _Feather;
            float _Size;
            float2 texCoords[9];
           
            struct appdata {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD;
            };
           
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD;
            };
 
            v2f vert(appdata IN) {
                v2f OUT;
                OUT.pos = UnityObjectToClipPos (IN.vertex);
                OUT.uv0 = IN.texcoord0;
                return OUT;
            }
 
     fixed4 frag(v2f IN) : COLOR {
           
        float4 avgColor; //will hold our averaged color from our sample points
        float2 texCoordsStep = 1.0/(float2(float(_ResolutionX),float(_ResolutionX))/float(_Size)); //width of "pixel region" in texture coords
        float2 pixelRegionCoords = frac(IN.uv0.xy/texCoordsStep); //x and y coordinates within "pixel region"
        float2 pixelBin = floor(IN.uv0.xy/texCoordsStep); //"pixel region" number counting away from base case
        float2 inPixelStep = texCoordsStep/3.0; //width of "pixel region" divided by 3 (for KERNEL_SIZE = 9, 3x3 square)
        float2 inPixelHalfStep = inPixelStep/2.0;
       
        //use offset (pixelBin * texCoordsStep) from base case (the lower left corner of billboard) to compute texCoords
     texCoords[0] = float2(inPixelHalfStep.x, inPixelStep.y*2.0 + inPixelHalfStep.y) + pixelBin * texCoordsStep;
     texCoords[1] = float2(inPixelStep.x + inPixelHalfStep.x, inPixelStep.y*2.0 + inPixelHalfStep.y) + pixelBin * texCoordsStep;
     texCoords[2] = float2(inPixelStep.x*2.0 + inPixelHalfStep.x, inPixelStep.y*2.0 + inPixelHalfStep.y) + pixelBin * texCoordsStep;
     texCoords[3] = float2(inPixelHalfStep.x, inPixelStep.y + inPixelHalfStep.y) + pixelBin * texCoordsStep;
     texCoords[4] = float2(inPixelStep.x + inPixelHalfStep.x, inPixelStep.y + inPixelHalfStep.y) + pixelBin * texCoordsStep;
     texCoords[5] = float2(inPixelStep.x*2.0 + inPixelHalfStep.x, inPixelStep.y + inPixelHalfStep.y) + pixelBin * texCoordsStep;
     texCoords[6] = float2(inPixelHalfStep.x, inPixelHalfStep.y) + pixelBin * texCoordsStep;
     texCoords[7] = float2(inPixelStep.x + inPixelHalfStep.x, inPixelHalfStep.y) + pixelBin * texCoordsStep;
     texCoords[8] = float2(inPixelStep.x*2.0 + inPixelHalfStep.x, inPixelHalfStep.y) + pixelBin * texCoordsStep;
   
     //take average of 9 pixel samples
     avgColor = tex2D(_MainTex, texCoords[0]) +
                         tex2D(_MainTex, texCoords[1]) +
                         tex2D(_MainTex, texCoords[2]) +
                         tex2D(_MainTex, texCoords[3]) +
                         tex2D(_MainTex, texCoords[4]) +
                         tex2D(_MainTex, texCoords[5]) +
                         tex2D(_MainTex, texCoords[6]) +
                         tex2D(_MainTex, texCoords[7]) +
                         tex2D(_MainTex, texCoords[8]);
                       
      avgColor /= float(9.0);
     
      //blend between fragments in the circle and out of the circle defining our "pixel region"
     //Equation of a circle: (x - h)^2 + (y - k)^2 = r^2
     float2 powers = pow(abs(pixelRegionCoords - 0.5),float2(2.0, 2.0));
     float radiusSqrd = pow(_Radius,2.0);
     float gradient = smoothstep(radiusSqrd-_Feather, radiusSqrd+_Feather, powers.x+powers.y);
     
      fixed4 fragcolor = lerp(avgColor, float4(0.1,0.1,0.1,0.0), gradient);
      return fragcolor;
            }
 
            ENDCG
        }
    }
}