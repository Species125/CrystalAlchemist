using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;

namespace CrystalAlchemist
{
    public class TrainingDummy : NonPlayer
    {
        public enum DummyState
        {
            ready,
            counting,
            showing
        }

        [BoxGroup("Dummy")]
        [SerializeField]
        private SpriteFillBar progress;

        [BoxGroup("Dummy")]
        [SerializeField]
        private GameObject timeBar;

        [BoxGroup("Dummy")]
        [SerializeField]
        private float time = 10f;

        [BoxGroup("Dummy")]
        [SerializeField]
        private float resultTime = 5f;

        [BoxGroup("Debug")]
        [SerializeField]
        private DummyState state = DummyState.ready;

        [BoxGroup("Debug")]
        [SerializeField]
        private float damage = 0f;

        [BoxGroup("Debug")]
        [SerializeField]
        private float countdown = 0;

        public override void Start()
        {
            base.Start();
            this.timeBar.SetActive(false);
        }

        public override void Update()
        {
            base.Update();

            if (this.state == DummyState.counting)
            {
                if (this.countdown > 0) CountingDown();
                else EndCountDown();
            }
        }

        public override void updateResource(CostType type, ItemGroup item, float value, bool showingDamageNumber)
        {
            if (this.state != DummyState.showing)
            {
                this.damage -= value;
                UpdateLifeMana(type, item, value, showingDamageNumber);
                if (this.values.life <= 0) this.values.life = this.values.maxLife;

                if (this.state != DummyState.counting) StartCountDown();
            }
        }

        private void StartCountDown()
        {
            this.state = DummyState.counting;
            this.timeBar.SetActive(true);
            this.countdown = this.time;
        }

        private void CountingDown()
        {
            this.countdown -= Time.deltaTime;
            this.progress.fillAmount((this.countdown / this.time));
        }

        private void EndCountDown()
        {
            this.state = DummyState.showing;

            this.progress.fillAmount(1);
            this.timeBar.SetActive(false);

            float DPS = this.damage / this.time;
            string text = FormatUtil.formatFloatToString(DPS, 1f) + " DPS!";
            ShowMiniDialog(text, this.resultTime - 1f);

            StartCoroutine(invincibleCo());
        }

        private IEnumerator invincibleCo()
        {
            yield return new WaitForSeconds(this.resultTime);
            this.damage = 0;
            RemoveAllStatusEffects();
            this.state = DummyState.ready;
        }
    }
}
