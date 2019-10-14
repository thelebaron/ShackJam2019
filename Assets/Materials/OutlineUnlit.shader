#warning Upgrade NOTE: unity_Scale shader variable was removed; replaced 'unity_Scale.w' with '1.0'

// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

    Shader "Unlit Shadows" {
    	Properties {
    		_Color ("Main Color", Color) = (1,1,1,1)
    		_MainTex ("Base (RGB)", 2D) = "white" {}
    		_OutlineColor ("Outline Colour", Color) = ( 0.0, 0.0, 0.0, 0.0)
            _OutlineAdjust ("Outline Adjust", Range(0.0, 1.0)) = 0.0
    	}
    	SubShader {
    		Tags {"Queue" = "Geometry" "RenderType" = "Opaque" "IgnoreProjector"="True"}
    		Pass {
    			Tags {"LightMode" = "ForwardBase"}
    			CGPROGRAM
    				#pragma vertex vert
    				#pragma fragment frag
    				#pragma multi_compile_fwdbase
    				#pragma fragmentoption ARB_fog_exp2
    				#pragma fragmentoption ARB_precision_hint_fastest
    				
    				#include "UnityCG.cginc"
    				#include "AutoLight.cginc"
    				
    				struct v2f
    				{
    					float4	pos			: SV_POSITION;
    					float2	uv			: TEXCOORD0;
    					fixed   outline : TEXCOORD1;
    					LIGHTING_COORDS(1,2)
    				};
    				float4 _MainTex_ST;
    				
    				
    				v2f vert (appdata_tan v)
    				{
    					v2f o;
    					
    					o.pos = UnityObjectToClipPos( v.vertex);
    					o.uv = TRANSFORM_TEX (v.texcoord, _MainTex).xy;
    					o.outline = dot(ObjSpaceViewDir(v.vertex), v.normal) / o.pos.w/ 1.0; // Ramp UV coord.
                    
    					TRANSFER_VERTEX_TO_FRAGMENT(o);
    					return o;
    				}
    				sampler2D _MainTex;
                    fixed4 _Color;
                    fixed4 _OutlineColor;
                    fixed _OutlineAdjust;
    				
    				
    				fixed4 frag(v2f i) : COLOR
    				{
    					fixed atten = LIGHT_ATTENUATION(i);	// Light attenuation + shadows.
    					//fixed atten = SHADOW_ATTENUATION(i); // Shadows ONLY.
    					//fixed4 texcol = tex2D (_MainTex, i.uv) * atten;
    					
    					
    					
    					
    					fixed4 c = tex2D ( _MainTex, i.uv )* atten; // Diffuse.
                        fixed o = step ( _OutlineAdjust, i.outline ); //Outline.
                        c *= o; // Apply outline to diffuse.
                        c += _OutlineColor * (1 - o); // Apply colour to outline, add to diffuse.
                        
                        return c;
                        //return texcol * _Color;//tex2D(_MainTex, i.uv) * atten;
    				}
    			ENDCG
    		}
    		Pass {
    			Tags {"LightMode" = "ForwardAdd"}
    			Blend One One
    			CGPROGRAM
    				#pragma vertex vert
    				#pragma fragment frag
    				#pragma multi_compile_fwdadd_fullshadows
    				#pragma fragmentoption ARB_fog_exp2
    				#pragma fragmentoption ARB_precision_hint_fastest
    				
    				#include "UnityCG.cginc"
    				#include "AutoLight.cginc"
    				
    				struct v2f
    				{
    					float4	pos			: SV_POSITION;
    					float2	uv			: TEXCOORD0;
    					fixed       outline : TEXCOORD2;
    					LIGHTING_COORDS(1,2)
    				};
    				float4 _MainTex_ST;
                    fixed4 _OutlineColor;
                    fixed _OutlineAdjust;
    				v2f vert (appdata_tan v)
    				{
    					v2f o;
    					
    					o.pos = UnityObjectToClipPos( v.vertex);
    					o.uv = TRANSFORM_TEX (v.texcoord, _MainTex).xy;
    					o.outline = dot(ObjSpaceViewDir(v.vertex), v.normal) / o.pos.w/ 1.0; // Ramp UV coord.
                    
    					TRANSFER_VERTEX_TO_FRAGMENT(o);
    					return o;
    				}
    				sampler2D _MainTex;
    				fixed4 frag(v2f i) : COLOR
    				{
    					fixed atten = LIGHT_ATTENUATION(i);	// Light attenuation + shadows.
    					//fixed atten = SHADOW_ATTENUATION(i); // Shadows ONLY.
    					
    					
    					fixed4 c = tex2D ( _MainTex, i.uv )* atten; // Diffuse.
                        fixed o = step ( _OutlineAdjust, i.outline ); //Outline.
                        c *= o; // Apply outline to diffuse.
                        c += _OutlineColor * (1 - o); // Apply colour to outline, add to diffuse.
                        
                        return c;
    					//return tex2D(_MainTex, i.uv) * atten;
    				}
    			ENDCG
    		}
    	}
    	FallBack "VertexLit"
    }