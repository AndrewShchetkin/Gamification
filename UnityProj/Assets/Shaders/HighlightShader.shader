Shader "Custom/Highlight" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		 _RendererColor ("RendererColor", Color) = (1,1,1,1)
		 _Flip ("Flip", Vector) = (1,1,1,1)
		 _AlphaTex ("External Alpha", 2D) = "white" {}
		 _EnableExternalAlpha ("Enable External Alpha", Float) = 0
	}

	SubShader {
		Tags { 
			"Queue"="Background"
			"IgnoreProjector"="True"
			"RenderType"="Transparent"
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		ZWrite Off
		ZTest Always
		Blend One OneMinusSrcAlpha

		Pass {
			ZTest Off
			Blend SrcAlpha OneMinusSrcAlpha
			CGPROGRAM
			#pragma vertex SpriteVert
			#pragma fragment SpriteFrag
			#pragma target 2.0
			#pragma multi_compile_instancing
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma multi_compile _ ETC1_EXTERNAL_ALPHA
			#include "UnitySprites.cginc"
			ENDCG
		}
	}
}