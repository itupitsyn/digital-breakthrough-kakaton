export const beautifyDuration = (duration: number) => {
  const rounded = Math.round(duration);
  const hours = Math.floor(rounded / 3600);
  const minutes = Math.floor((rounded - hours * 3600) / 60);
  const seconds = rounded - hours * 3600 - minutes * 60;

  let result = "";
  if (seconds) result += `${seconds}c`;
  if (minutes) result = `${minutes}м ${result}`;
  if (hours) result = `${hours}ч ${result}`;

  return result;
};
