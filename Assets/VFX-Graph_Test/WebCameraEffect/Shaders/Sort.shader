Shader "Hidden/Sort"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            // インスペクタに表示したときにわかりやすいように名前を付けておく
            Name "Update"

            CGPROGRAM
            
            // UnityCustomRenderTexture.cgincをインクルードする
            #include "UnityCustomRenderTexture.cginc"

            // 頂点シェーダは決まったものを使う
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag

            sampler2D _MainTex;

            int MaxCount(int width, int height) 
            {
                int maxCount = 0;
                for (int y = 0; y < height; y ++) 
                {
                    for (int x = 0; x < width; x ++)
                    {
                        half4 col = tex2D(_MainTex, float2(x / (float)width, y / (float)height));
                        if (length(col.rgb) != 0) 
                        {
                            maxCount++;
                        }
                    }
                }
                return maxCount;
            }

            // v2f構造体は決まったものを使う
            half4 frag(v2f_customrendertexture i) : SV_Target
            {
                int width = 124;
                int height = 124;
                float2 uv = i.globalTexcoord;
                int2 pos = uv * int2(width, height);
                int index = pos.y * width + pos.x;
                int maxCount = MaxCount(width, height);
                int count = 0;
                int colCount = index % maxCount;
                for (int y = 0; y < height; y ++) 
                {
                    for (int x = 0; x < width; x ++)
                    {
                        half4 col = tex2D(_MainTex, float2(x / (float)width, y / (float)height));
                        if (length(col.rgb) != 0) 
                        {
                            if (colCount == count) 
                            {
                                return half4(x / (float)width, y / (float)height, 0, 1.0);
                            }
                            count ++;
                        }
                    }
                }
                // return half4(0.0, 0.0, 0.0, 1.0);
                return tex2D(_SelfTexture2D, uv);
            }

            ENDCG
        }
    }
}
