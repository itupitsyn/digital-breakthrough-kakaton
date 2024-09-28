import { Video } from "@/model/video";
import { faker } from "@faker-js/faker";

export const getRandomVideos = (count: number) => {
  const data: Video[] = [];
  const states: ("like" | "dislike" | "skip")[] = ["like", "dislike", "skip"];
  for (let i = 0; i < count; i += 1) {
    const randomObj: Video = {
      video_id: faker.number.int(),
      title: faker.word.words({ count: Math.ceil(Math.random() * 10) }),
      description: faker.lorem.paragraph(),
      preview: faker.image.urlLoremFlickr(),
      category: faker.word.words({ count: Math.ceil(Math.random() * 3) }),
      v_pub_datetime: faker.date.past().valueOf(),
      v_likes: faker.number.int(),
      v_dislikes: faker.number.int(),
      v_duration: faker.number.int({ max: 36000 }),
      state: states[Math.floor(Math.random() * states.length)],
    };

    data.push(randomObj);
  }

  return data;
};
