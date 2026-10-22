public enum EArtifactDealStepDelay
{
	[StepMemo("초기 세팅: 시작 말풍선(대사) 표시 시간")]
	InitialSetupTextBubbleDuration = 100,
	[StepMemo("초기 세팅: 말풍선 종료 후 대기")]
	InitialSetupAfterTextBubble = 200,
	[StepMemo("초기 세팅: 남은 협상 횟수 카운트 트윈 시간")]
	InitialSetupRemainCountTween = 300,
	[StepMemo("초기 세팅: 남은 횟수 연출 후 대기")]
	InitialSetupAfterRemainCount = 400,
	[StepMemo("초기 세팅: 손패 분배(딜) 후 대기")]
	InitialSetupAfterHandDeal = 500,
	[StepMemo("초기 세팅: '거래 시작' 타이틀 표시 후 대기")]
	InitialSetupAfterDealStartTitle = 600,
	[StepMemo("손님 리액션(망설임) 지연 — 성공률 Mid 구간")]
	HesitateMidDuration = 650,
	[StepMemo("손님 리액션(망설임) 지연 — 성공률 Low 구간 (High 구간은 망설임 없음)")]
	HesitateLowDuration = 660,
	[StepMemo("결과 단계 ①: 결과 대사 표시 후 대기(x)")]
	CardResultBubbleWait = 700,
	[StepMemo("결과 단계 ②: Suggest(성공/실패) 표시 유지 시간 — 이 시간 후 논블로킹으로 끔(숫자 연출과 겹침)")]
	CardResultSuggestDuration = 720,
	[StepMemo("결과 단계 ③: Suggest 표시 후 → 가격/태그 숫자 연출 시작까지 대기 (suggest 켜진 채 겹쳐 시작)")]
	CardResultSuggestToNumberDelay = 740,
	[StepMemo("카드 발동(PlayCardUp) → 사용 완료(PlayComplete) 까지 목표 간격. 결과/효과 큐와 별개인 독립 타이머")]
	CardUpToCompleteOffset = 750,
	[StepMemo("카드 사용 중: 손패 동기화 후 대기")]
	CardPlayingAfterHandSync = 800,
	[StepMemo("태그 갱신: 싫어하는 태그 제거 후 대기")]
	TagUpdateAfterRemove = 900,
	[StepMemo("태그 갱신: 선호 태그 추가 후 대기")]
	TagUpdateAfterAdd = 1000,
	[StepMemo("새로 등장하는 요구 태그 슬롯의 ApearFx 페이드/스케일 등장 시간")]
	SlotAddAnimationDuration = 1050,
	[StepMemo("사라지는 태그: Cancel(취소선) 오브젝트를 켠 채 hold 후 슬롯 비활성")]
	RemoveSlotCancelHold = 1100,
	[StepMemo("+/- floating 텍스트 Animator(ArtifactDealPriceUp/Down) 가 떠올랐다 다음 단계로 넘어가기 전까지 표시 시간")]
	PriceChangeFloatingDuration = 1150,
	[StepMemo("거래가(Coin) 텍스트 값 트윈 시간 (changeAnimator 와 별개)")]
	DealPriceTweenDuration = 1160,
	[StepMemo("거래 경험치 텍스트 값 트윈 시간")]
	DealExpTweenDuration = 1170,
	[StepMemo("가격/경험치 갱신 연출 후 대기")]
	PriceUpdateWait = 1200,
	[StepMemo("카드 플레이/딜 셋업 완료 후 AwaitAction(클릭 가능) 진입 직전 — 카드 점등(불 들어오기) 애니가 클릭 게이트보다 앞서도록 주는 lead 시간")]
	CardUsableLightUpLead = 1250,
	[StepMemo("리롤: State1 애니메이션 시작 → 새 손패 세팅(애니메이션 중간)")]
	RerollHandSetDelay = 1300,
	[StepMemo("리롤: 손패 세팅 → 사용 가능 카드 강조 + 리롤 횟수 연출")]
	RerollUsableDelay = 1310,
	[StepMemo("리롤: 사용 가능 업데이트/리롤 횟수 연출 시작 → AwaitAction 전환")]
	RerollAfterUpdate = 1320,
	[StepMemo("협상 횟수 0 강조 애니(RemainPlayCount) 1회 실행 후 → 소진 대사 전 대기(x)")]
	NoMorePlayCountAlertWait = 1390,
	[StepMemo("협상 횟수 모두 소진 안내(말풍선) 연출 시간")]
	NoMorePlayCountWait = 1400,
	[StepMemo("협상 체결: 체결 대사(말풍선) 표시 후 → 완료 팝업 오픈 전 대기")]
	DealAcceptedBubbleWait = 1500,
	[StepMemo("협상 포기: 포기 대사(말풍선) 표시 후 → BackOut 팝업 오픈 전 대기")]
	DealGiveUpBubbleWait = 1510
}
