//Options.jsx
import { useState } from 'react';
import styles from './CSS/Options.module.css';

export default function Options({ title, allbts, clickme }) {
  const optionbts = allbts.map(x => {
    return (
      <button
        key={x}
        onClick={() => {
          clickme(x);
        }}>
        {x}
      </button>
    );
  });

  return (
    <div className={styles.options}>
      <h4>{title}</h4>
      {optionbts}
    </div>
  );
}
