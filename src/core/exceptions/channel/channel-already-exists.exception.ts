import { AlreadyExistsException } from "../base/already-exists.exception";

export class ChannelAlreadyExistsException extends AlreadyExistsException {
  constructor(key: string, value: object) {
    super("Channel", key, value);
  }
}
