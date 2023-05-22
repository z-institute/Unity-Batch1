import styles from './CSS/Avatar.module.css';
import { useEffect } from 'react';

export default function Avatar({ appearance }) {
  function drawAlpaca(data) {
    return Object.entries(data).map(([key, value], index) =>
      value === 'None' || value === '' ? (
        ''
      ) : (
        <img
          key={index}
          className={styles[key.toLocaleLowerCase()]}
          src={require(`images/${key.toLocaleLowerCase()}/${value.toLocaleLowerCase()}.png`)}
          alt={key}
          id={key.toLocaleLowerCase() + value.toLocaleLowerCase()}
        />
      )
    );
  }

  //Canvas 產生圖片
  useEffect(() => {
    // Canvas引用圖片

    const canvas = document.getElementById('canvas');
    const context = canvas.getContext('2d');

    const backgrounds = document.querySelector("[id^='backgrounds']");
    const neck = document.querySelector("[id^='neck']");
    const hair = document.querySelector("[id^='hair']");
    const ears = document.querySelector("[id^='ears']");
    const leg = document.querySelector("[id^='leg']");
    const nose = document.getElementById('nose');
    const mouth = document.querySelector("[id^='mouth']");
    const eyes = document.querySelector("[id^='eyes']");
    const accessories = document.querySelector("[id^='accessories']");

    context.drawImage(backgrounds, 0, 0);
    context.drawImage(ears, 0, 0);
    context.drawImage(neck, 0, 0);
    context.drawImage(hair, 0, 0);
    context.drawImage(leg, 0, 0);
    context.drawImage(nose, 0, 0);
    context.drawImage(mouth, 0, 0);
    context.drawImage(eyes, 0, 0);
    if (accessories) {
      context.drawImage(accessories, 0, 0);
    }

    console.log(appearance);
  }, [appearance]);

  return (
    <div className={styles.avatar}>
      <canvas
        className={styles.canvas}
        height='720'
        width='720'
        id='canvas'></canvas>

      <div className={styles.avatarImg}>
        {drawAlpaca(appearance)}
        <img
          className={styles.nose}
          src={require('images/nose.png')}
          alt='nose'
          id='nose'
        />
      </div>
    </div>
  );
}

