Shader "Custom/MainShader"
{
    SubShader 
    {
        Tags { "RenderType"="Opaque"}
        LOD 200

        Pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"
            
            //vart 
            struct appdata{
                float4 vertex: POSITION;
            };


            //frag
            struct v2f
            {
                float4 vertex: SV_POSITION;
            };

            v2f vert(appdata v){
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            };

            fixed4 frag(v2f i): SV_TARGET
            {   
                float r = sin(_Time * 100);
                return fixed4(r,0.5,0.5,1);
            };

            ENDCG
        }

    }
}