import Avatar from './Avatar.jsx';

import Options from './Options.jsx';
import { useState } from 'react';
import styles from './CSS/Main.module.css';

export default function Main() {
  const allItems = {
    Ears: ['Default', 'Tilt-backward', 'Tilt-forward'],
    Eyes: ['Default', 'Angry', 'Naughty', 'Panda', 'Smart', 'Star'],
    Hair: ['Default', 'Curls', 'Short', 'Bang', 'Elegant', 'Quiff'],
    Leg: [
      'Default',
      'Bubble-tea',
      'Cookie',
      'Game-console',
      'Tilt-backward',
      'Tilt-forward'
    ],
    Mouth: ['Default', 'Astonished', 'Eating', 'Laugh', 'Tongue'],
    Neck: ['Default', 'Thick', 'Bend-backward', 'Bend-forward'],
    Accessories: ['None', 'Earings', 'Flower', 'Glasses', 'Headphone'],
    Backgrounds: [
      'Blue50',
      'Blue60',
      'Blue70',
      'DarkBlue30',
      'DarkBlue50',
      'DarkBlue70',
      'Green50',
      'Green60',
      'Green70',
      'Grey40',
      'Grey70',
      'Grey80',
      'Red50',
      'Red60',
      'Red70',
      'Yellow50',
      'Yellow60',
      'Yellow70'
    ]
  };

  const [selectedCategory, setSelectedCategory] = useState('Ears');

  function pageHandler(value) {
    setAcspage(allItems[value]);
    setSelectedCategory(value);
  }

  const [acspage, setAcspage] = useState(allItems.Ears);

  const [alpaca, setAlpaca] = useState({
    Ears: 'Default',
    Eyes: 'Default',
    Hair: 'Default',
    Leg: 'Default',
    Mouth: 'Default',
    Neck: 'Default',
    Accessories: '',
    Backgrounds: 'Blue50'
  });

  function acsHandler(value) {
    setAlpaca(prevState => ({
      ...prevState,
      [selectedCategory]: value
    }));
    // console.log('acs:', alpaca);
  }

  function handleDownload() {
    const canvas = document.getElementById('canvas');
    const anchor = document.createElement('a');
    anchor.href = canvas.toDataURL('image/png');
    anchor.download = 'AlpacaImage.png';
    anchor.click();
  }

  return (
    <div className={styles.main}>
      <div className={styles.mainLeft}>
        <Avatar appearance={alpaca} />
        <div className={styles.mainLeftDown}>
          <button>Random</button>
          <button onClick={handleDownload}>Donwload</button>
        </div>
      </div>
      <div className={styles.mainRight}>
        <Options
          title={'Accessorize the Alpaca!'}
          allbts={Object.keys(allItems)}
          clickme={pageHandler}
        />
        <Options
          title={'Style'}
          allbts={acspage}
          clickme={acsHandler}
        />
      </div>
    </div>
  );
}
