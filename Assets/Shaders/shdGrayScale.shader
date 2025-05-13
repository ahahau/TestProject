Shader "Unlit/shdGrayScale" //언릿 
{
    Properties // 인스펙터에서 표시되는 머티리얼 프로퍼티
    {
        // 텍스처 프로퍼티 (_MainTex)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // 셰이더 태그 설정 (투명 렌더링)
        Tags { "RenderType"="Transparent" "Opaque" = "Transparent"}
        // 알파 블렌딩 설정 (SrcAlpha를 기준으로 투명도 조절)
        Blend SrcAlpha OneMinusSrcAlpha
        // 깊이 버퍼 쓰기 비활성화 (투명 오브젝트는 깊이 정보를 쓰지 않음)
        ZWrite Off
        // 후면 컬링 비활성화 (양면 렌더링 가능)
        Cull Off
         
        Pass
        {
            CGPROGRAM
            #pragma vertex vert// 버텍스 셰이더 지정
            #pragma fragment frag // 프래그먼트 셰이더 지정

            #include "UnityCG.cginc" // UnityCG 라이브러리 포함

            // 버텍스 셰이더 입력 구조체
            struct appdata
            {
                float4 vertex : POSITION;// 정점 위치
                float2 uv : TEXCOORD0;// 텍스처 좌표
            };

    // 버텍스 셰이더 출력 및 프래그먼트 셰이더 입력 구조체
            struct v2f
            {
                float2 uv : TEXCOORD0; // 텍스처 좌표
                float4 vertex : SV_POSITION;// 클립 공간 위치
            };

            // 텍스처 샘플러 및 텍스처 스케일 변환 매트릭스
            sampler2D _MainTex;
            float4 _MainTex_ST;

            // 버텍스 셰이더 (정점 변환 및 UV 좌표 설정)
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); // 오브젝트 정점을 클립 공간으로 변환
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); // 텍스처 좌표 변환
                return o;
            }

            // 프래그먼트 셰이더 (텍스처 샘플링 및 흑백 변환)
            fixed4 frag (v2f i) : SV_Target
            {
                // 텍스처 색상 샘플링
                fixed4 col = tex2D(_MainTex, i.uv);
                // 그레이스케일 변환 (가중 평균법 적용) 
                float grey = dot(col.rgb, float3(0.299, 0.587, 0.114));
                /*
                코드에서 dot(col.rgb, float3(0.299, 0.587, 0.114))은 가중 평균법을 사용하여 컬러 이미지를 흑백으로 변환하는 연산입니다.

공식:

grey=(0.299×R)+(0.587×G)+(0.114×B)

R(빨강): 0.299
G(초록): 0.587
B(파랑): 0.114

왜 이런 가중치를 사용할까?
이 값들은 사람의 눈이 색을 인식하는 방식을 반영한 것입니다.

**녹색(G)**에 가장 높은 가중치를 주는 이유: 인간의 눈은 녹색에 가장 민감하기 때문입니다.
**파란색(B)**이 가장 낮은 가중치를 받는 이유: 인간의 눈이 파란색을 가장 덜 민감하게 인식하기 때문입니다.
이 방식은 디지털 이미지 프로세싱과 그래픽스에서 표준적인 방법으로, 흑백 필터를 적용할 때 일반적으로 사용됩니다.
                */
                // 변환된 흑백 색상 반환 (알파값 유지)
                return fixed4(grey, grey, grey, col.a);
            }
            ENDCG
        }
    }
        // 지원되지 않을 경우 기본 Diffuse 셰이더 사용
        FallBack "Diffuse"
}
